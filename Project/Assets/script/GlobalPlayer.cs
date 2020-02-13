using System;
using Mugen;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpriteType
{
	Default = 0,
}

public class GlobalPlayer
{
	private string m_PlayerName = string.Empty;
	private PlayerConfig m_PlayerConfig = null;
	private AirConfig m_AirConfig = null;
	private CNSConfig m_CNSConfig = null;
	private CmdConfig m_CmdConfig = null;
	private LuaCnsConfig m_LuaConfig = null;

	public LuaCnsConfig LuaCfg
	{
		get
		{
			return m_LuaConfig;
		}
	}

	public CmdConfig CmdCfg
	{
		get {
            if (m_LuaConfig != null)
                return m_LuaConfig.CmdCfg;
			return m_CmdConfig;
		}
	}

    public CNSConfig CnsCfg
    {
        get
        {
            if (m_CNSConfig == null)
            {
                if (m_LuaConfig != null)
                    return m_LuaConfig.CnsCfg;
            }
            return m_CNSConfig;
        }
    }

	public AirConfig AirCfg
	{
		get
		{ 
			return m_AirConfig;
		}
	}

	public string PlayerName {
		get {
			return m_PlayerName;
		}
	}

	public PlayerConfig PlayerCfg
	{
		get {
			return m_PlayerConfig;
		}
	}

	private bool Init(string playerName, out GlobalPlayerLoaderResult result, string cnsName = "")
	{
		Clear ();
		result = GlobalPlayerLoaderResult.None;
		m_PlayerName = playerName;
		if (string.IsNullOrEmpty (playerName)) {
			result = GlobalPlayerLoaderResult.ParamError;
			return false;
		}
		try
		{
			m_PlayerConfig = new PlayerConfig();
			m_PlayerConfig.LoadPlayer (playerName);
		} catch (Exception e) {
			#if DEBUG
			Debug.LogError(e.ToString());
			#endif
			Clear ();
			result = GlobalPlayerLoaderResult.PlayerConfigError;
			return false;
		}

		if (!m_PlayerConfig.IsVaild) {
			Clear ();
			result = GlobalPlayerLoaderResult.PlayerConfigError;
			return false;
		}
		try
		{
			string airName = string.Empty;
			if (m_PlayerConfig != null && m_PlayerConfig.Files != null)
			{
				airName = m_PlayerConfig.Files.anim;
				airName = GlobalConfigMgr.GetConfigFileNameNoExt(airName);
			}
			m_AirConfig = new AirConfig (playerName, airName);
			if (!m_AirConfig.IsVaild)
			{
				Clear();
				result = GlobalPlayerLoaderResult.AirConfigError;
				return false;
			}
		} catch (Exception e) {
			#if DEBUG
			Debug.LogError(e.ToString());
			#endif
			Clear ();
			result = GlobalPlayerLoaderResult.AirConfigError;
			return false;
		}

        // 判断LUA
        bool isLua = false;
        try
        {
            if (m_PlayerConfig != null && m_PlayerConfig.Files != null && m_PlayerConfig.Files.HasLuaFile)
            {
                isLua = true;
                m_LuaConfig = new LuaCnsConfig();
                if (!m_LuaConfig.LoadFromFile(m_PlayerConfig.Files.lua))
                {
                    result = (GlobalPlayerLoaderResult)((int)result | (int)GlobalPlayerLoaderResult.LUAConfigError);
                }
            }
        }
        catch (Exception e)
        {
#if DEBUG
            Debug.LogError(e.ToString());
#endif
            result = (GlobalPlayerLoaderResult)((int)result | (int)GlobalPlayerLoaderResult.LUAConfigError);
        }

		//---------------------------- 加载Cmd
        if (!isLua)
        {
            try
            {
                if (m_PlayerConfig != null && m_PlayerConfig.Files != null)
                {
                    string cmdName = m_PlayerConfig.Files.cmd;
                    if (string.IsNullOrEmpty(cmdName))
                    {
                        cmdName = playerName;
                    }
                    else
                    {
                        cmdName = GlobalConfigMgr.GetConfigFileNameNoExt(cmdName);
                    }
                    string fileName = string.Format("{0}@{1}/{2}.cmd.txt", AppConfig.GetInstance().PlayerRootDir, playerName, cmdName);
                    m_CmdConfig = new CmdConfig();
                    if (!m_CmdConfig.LoadFromFile(fileName))
                    {
                        result = GlobalPlayerLoaderResult.CmdConfigError;
                    }
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.LogError(e.ToString());
#endif
                result = GlobalPlayerLoaderResult.CmdConfigError;
            }
        }

		

		//--------------------------- 最后加载cns
		if (!isLua)
		{
		try
		{
			if (string.IsNullOrEmpty(cnsName))
			{
				if (m_PlayerConfig == null || m_PlayerConfig.Files == null)
					cnsName = playerName;
				else
				{
					cnsName = m_PlayerConfig.Files.cns;
					cnsName = GlobalConfigMgr.GetConfigFileNameNoExt(cnsName);
				}
			}
			string fileName = string.Format("{0}@{1}/{2}.cns.txt", AppConfig.GetInstance().PlayerRootDir, playerName, cnsName);
			m_CNSConfig = new CNSConfig();
			if (!m_CNSConfig.LoadFromFile(fileName))
			{
				//Clear();
				result = (GlobalPlayerLoaderResult)((int)result | (int)GlobalPlayerLoaderResult.CNSConfigError);
				//return false;
			}
		} catch (Exception e) {
			#if DEBUG
			Debug.LogError(e.ToString());
			#endif
			//Clear ();
				result =  (GlobalPlayerLoaderResult)((int)result | (int)GlobalPlayerLoaderResult.CNSConfigError);
			//return false;
		}
		}

		if (result == GlobalPlayerLoaderResult.None)
			result = GlobalPlayerLoaderResult.Ok;
		return true;
	}

	private void Clear()
	{
		m_AirConfig = null;
		m_PlayerConfig = null;
		m_CNSConfig = null;
		m_CmdConfig = null;
        if (m_LuaConfig != null)
        {
            m_LuaConfig.Dispose();
            m_LuaConfig = null;
        }
	}

	public static GlobalPlayer CreatePlayer(string playerName, out GlobalPlayerLoaderResult result, string cnsName = "")
	{
		if (string.IsNullOrEmpty (playerName)) {
			result = GlobalPlayerLoaderResult.ParamError;
			return null;
		}
		GlobalPlayer player = new GlobalPlayer ();
		bool ret = player.Init (playerName, out result);
		if (!ret)
			return null;
		return player;
	}

	// 创建角色显示
	public GameObject CreatePlayerDisplay(DefaultLoaderPlayer loaderPlayer, InputPlayerType playerType, bool Shader_RGB_Zero_Alpha_One = true, float scale = 1.0f)
	{
		if (m_PlayerConfig == null || !m_PlayerConfig.IsVaild || m_AirConfig == null || !m_AirConfig.IsVaild)
			return null;
		if (playerType == InputPlayerType.none) {
			Debug.LogError ("[CreatePlayerDisplay] InputPlayerType is None");
			return null;
		}

		if (PlayerControls.GetInstance ().GetPlayer (playerType) != null) {
			Debug.LogError ("[CreatePlayerDisplay] InputPlayerType is Exists");
			return null;
		}

		GameObject obj = new GameObject (m_PlayerName, typeof(PlayerDisplay), typeof(PlayerPartMgr));
        PlayerDisplay displayer = obj.GetComponent<PlayerDisplay>();
		displayer.Init(loaderPlayer, playerType, Shader_RGB_Zero_Alpha_One, scale);
		return obj;
	}
}
