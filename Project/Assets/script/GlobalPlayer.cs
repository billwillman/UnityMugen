using System;
using Mugen;
using UnityEngine;
using System.Collections;

public class GlobalPlayer
{
	private string m_PlayerName = string.Empty;
	private PlayerConfig m_PlayerConfig = null;
	private AirConfig m_AirConfig = null;
	private CNSConfig m_CNSConfig = null;

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
				airName = System.IO.Path.GetFileNameWithoutExtension(airName);
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


		//--------------------------- 最后加载cns
		try
		{
			if (string.IsNullOrEmpty(cnsName))
			{
				if (m_PlayerConfig == null || m_PlayerConfig.Files == null)
					cnsName = playerName;
				else
				{
					cnsName = m_PlayerConfig.Files.cns;
					cnsName = System.IO.Path.GetFileNameWithoutExtension(cnsName);
				}
			}
			string fileName = string.Format("{0}@{1}/{2}.cns.txt", AppConfig.GetInstance().PlayerRootDir, playerName, cnsName);
			m_CNSConfig = new CNSConfig();
			if (!m_CNSConfig.LoadFromFile(fileName))
			{
				//Clear();
				result = GlobalPlayerLoaderResult.CNSConfigError;
				//return false;
			}
		} catch (Exception e) {
			#if DEBUG
			Debug.LogError(e.ToString());
			#endif
			//Clear ();
			result = GlobalPlayerLoaderResult.CNSConfigError;
			//return false;
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
	public GameObject CreatePlayerDisplay()
	{
		if (m_PlayerConfig == null || !m_PlayerConfig.IsVaild || m_AirConfig == null || !m_AirConfig.IsVaild)
			return null;
		GameObject obj = new GameObject (m_PlayerName, typeof(PlayerDisplay));
		return obj;
	}
}
