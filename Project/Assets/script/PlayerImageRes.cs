using UnityEngine;
using System.Collections;
using Mugen;

	// 角色资源
[RequireComponent(typeof(DefaultLoaderPlayer))]
public class PlayerImageRes : MonoBehaviour {

	private ImageLibrary m_ImgLib = null;

	public bool Is32BitPallet = true;

	public bool LoadOk = false;

    void OnApplicationQuit()
    {
        Clear();
    }

    public void Clear()
    {
        LoadOk = false;
        if (m_ImgLib != null)
        {
            m_ImgLib.Dispose();
            m_ImgLib = null;
        }
    }

    public ImageLibrary ImgLib
    {
        get
        {
            return m_ImgLib;
        }
    }

	void OnDestroy()
	{
        if (!AppConfig.IsAppQuit)
		    Clear ();
	}

	public void Init()
	{
		Clear ();

		m_ImgLib = new ImageLibrary (Is32BitPallet);
		DefaultLoaderPlayer loadPlayer = GetComponent<DefaultLoaderPlayer> ();
		var player = loadPlayer.GetGlobalPayer ();
		string spriteName = GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.sprite);
        LoadOk = m_ImgLib.LoadChar(loadPlayer.GetPlayerName(), player.AirCfg, spriteName); 
	}
}
