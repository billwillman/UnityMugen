using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;

[RequireComponent(typeof(SndLoader))]
[RequireComponent(typeof(PlayerImageRes))]
public class DefaultLoaderPlayer : MonoBehaviour {
	public string PlayerName = string.Empty;
	public string CnsName = string.Empty;
    //public InputPlayerType PlayerType = InputPlayerType.none;
	public bool Shader_RGB_Zero_Alpha_One = true;

    private PlayerImageRes m_ImageLibrary = null;
    private SndLoader m_SndLoader = null;
    

    // 清理图片资源
    public void ClearImageRes()
    { 
        if (m_ImageLibrary != null)
        {
            m_ImageLibrary.Clear();
        }
    }

    public byte[] GetSoundBuf(int group, int index)
    {
        if (group < 0 || index < 0)
            return null;

        var sndLoader = this.SoundLoader;
        if (sndLoader == null)
            return null;


        if (!sndLoader.IsInited)
        {
            if (!sndLoader.Load(this))
                return null;
        }
        byte[] ret = sndLoader.GetSoundBuf(group, index);
        return ret;
    }

    public bool LoadSounds()
    {
        var snd = this.SoundLoader;
        if (snd == null)
            return false;
        return snd.Load(this);
    }

    public Dictionary<KeyValuePair<int, int>, byte[]>.Enumerator GetSoundIter()
    {
        var sndLoader = this.SoundLoader;
        if (sndLoader == null)
            return new Dictionary<KeyValuePair<int, int>, byte[]>.Enumerator();
        return sndLoader.GetSoundIter();
    }

    public SndLoader SoundLoader
    {
        get
        {
            if (m_SndLoader == null)
                m_SndLoader = GetComponent<SndLoader>();
            return m_SndLoader;
        }
    }


    public PlayerImageRes ImageRes
    {
        get
        {
            if (m_ImageLibrary == null)
                m_ImageLibrary = GetComponent<PlayerImageRes>();
            return m_ImageLibrary;
        }
    }

	public string GetPlayerName()
	{
		string playerName = this.PlayerName;
		if (string.IsNullOrEmpty (playerName))
			playerName = gameObject.name;
		return playerName;
	}

	public GlobalPlayer GetGlobalPayer()
	{
        if (!GlobalConfigMgr.GetInstance().HasLoadPlayer(this))
            return null;
		GlobalPlayerLoaderResult result;
		var ret = GlobalConfigMgr.GetInstance ().LoadPlayer (this, out result);
		return ret;
	}


	public GlobalPlayerLoaderResult LoadResult = GlobalPlayerLoaderResult.None;
	public List<string> CnsNameList = null;
	public List<string> PalNameList = null;
	public string LoadAnim = string.Empty;

    public void _OnLoadCns()
    {
        
    }

	public void AddCnsName(string name)
	{
		if (string.IsNullOrEmpty (name))
			return;
		if (CnsNameList == null)
			CnsNameList = new List<string> ();
		if (!CnsNameList.Contains(name))
			CnsNameList.Add (name);
	}

	public void AddPalName(string name)
	{
		if (string.IsNullOrEmpty (name))
			return;
		if (PalNameList == null)
			PalNameList = new List<string> ();
		if (!PalNameList.Contains(name))
			PalNameList.Add (name);
	}
}
