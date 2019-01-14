using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;

[RequireComponent(typeof(PlayerImageRes))]
public class DefaultLoaderPlayer : MonoBehaviour {
	public string PlayerName = string.Empty;
	public string CnsName = string.Empty;
    private PlayerImageRes m_ImageLibrary = null;

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
