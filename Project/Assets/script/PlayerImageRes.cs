using UnityEngine;
using System.Collections;
using Mugen;

	// 角色资源
[RequireComponent(typeof(DefaultLoaderPlayer))]
public class PlayerImageRes : MonoBehaviour {

	private ImageLibrary m_ImgLib = null;

	public bool Is32BitPallet = true;

	public bool LoadOk = false;

	public void Clear()
	{
		LoadOk = false;
		if (m_ImgLib != null) {
			m_ImgLib.Dispose ();
			m_ImgLib = null;
		}
	}

	void OnDestroy()
	{
		Clear ();
	}

	public void Init()
	{
		Clear ();

		m_ImgLib = new ImageLibrary (Is32BitPallet);
		DefaultLoaderPlayer loadPlayer = GetComponent<DefaultLoaderPlayer> ();
		var player = loadPlayer.GetGlobalPayer ();
		LoadOk = m_ImgLib.LoadChar (loadPlayer.GetPlayerName (), player.AirCfg, player.PlayerCfg.Files.sprite); 
	}
}
