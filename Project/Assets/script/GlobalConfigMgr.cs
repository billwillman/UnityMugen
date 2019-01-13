using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GlobalPlayerLoaderResult
{
	CNSConfigError = -4,
	AirConfigError = -3,
	PlayerConfigError = -2,
	ParamError = -1,
	Ok = 0,
	None = 1,
}

public class GlobalConfigMgr : MonoSingleton<GlobalConfigMgr> {
	private Dictionary<string, GlobalPlayer> m_PlayerDict = new Dictionary<string, GlobalPlayer>();

	// 默认加载的PlayerName, 可用于测试
	public List<DefaultLoaderPlayer> m_DefaultLoaderPlayers = null;

	public GlobalPlayer LoadPlayer(DefaultLoaderPlayer loaderPlayer, out GlobalPlayerLoaderResult result)
	{
		if (loaderPlayer == null) {
			result = GlobalPlayerLoaderResult.ParamError;
			return null;
		}
		string playerName = loaderPlayer.GetPlayerName();
		return LoadPlayer (playerName, out result, loaderPlayer.CnsName);
	}

	public GlobalPlayer LoadPlayer(string playerName, out GlobalPlayerLoaderResult result, string cnsName = "")
	{
		result = GlobalPlayerLoaderResult.Ok;
		GlobalPlayer player;
		if (m_PlayerDict.TryGetValue (playerName, out player) && player != null)
			return player;
		player = GlobalPlayer.CreatePlayer (playerName, out result, cnsName);
		if (player != null) {
			m_PlayerDict [playerName] = player;
		}
		return player;
	}

	public void Clear()
	{
		m_PlayerDict.Clear ();
	}

	private void AddCnsList(DefaultLoaderPlayer loaderPlayer, GlobalPlayer player)
	{
		if (player == null || player.PlayerCfg == null || player.PlayerCfg.Files == null) {
			loaderPlayer.CnsNameList = null;
		} else {
			var files = player.PlayerCfg.Files;
			string name = Path.GetFileNameWithoutExtension (files.cns);
			loaderPlayer.AddCnsName (name);
			name = Path.GetFileNameWithoutExtension (files.st);
			loaderPlayer.AddCnsName (name);
			name = Path.GetFileNameWithoutExtension (files.st2);
			loaderPlayer.AddCnsName (name);
			name = Path.GetFileNameWithoutExtension (files.st3);
			loaderPlayer.AddCnsName (name);
			name = Path.GetFileNameWithoutExtension (files.st4);
			loaderPlayer.AddCnsName (name);
		}
	}

	private void AttachAnim(DefaultLoaderPlayer loaderPlayer, GlobalPlayer player)
	{
		if (player == null || player.PlayerCfg == null || player.PlayerCfg.Files == null) {
			loaderPlayer.LoadAnim = string.Empty;
		} else {
			var files = player.PlayerCfg.Files;
			loaderPlayer.LoadAnim = Path.GetFileNameWithoutExtension(files.anim);
		}
	}

	private void AttachPals(DefaultLoaderPlayer loaderPlayer, GlobalPlayer player)
	{
		if (player == null || player.PlayerCfg == null || player.PlayerCfg.Files == null)
			loaderPlayer.PalNameList = null;
		else {
			var files = player.PlayerCfg.Files;
			string name = Path.GetFileNameWithoutExtension(files.pal1);
			loaderPlayer.AddPalName (name);
			name = Path.GetFileNameWithoutExtension(files.pal2);
			loaderPlayer.AddPalName (name);
			name = Path.GetFileNameWithoutExtension(files.pal3);
			loaderPlayer.AddPalName (name);
			name = Path.GetFileNameWithoutExtension(files.pal4);
			loaderPlayer.AddPalName (name);
			name = Path.GetFileNameWithoutExtension(files.pal5);
			loaderPlayer.AddPalName (name);
			name = Path.GetFileNameWithoutExtension(files.pal6);
			loaderPlayer.AddPalName (name);
		}
	}

	void Start()
	{
		if (m_DefaultLoaderPlayers != null) {
			for (int i = 0; i < m_DefaultLoaderPlayers.Count; ++i) {
				var defaultPlayer = m_DefaultLoaderPlayers [i];
				if (defaultPlayer != null) {
					GlobalPlayerLoaderResult result;
					var player = LoadPlayer (defaultPlayer, out result);
					#if UNITY_EDITOR
					defaultPlayer.LoadResult = result;
					AttachAnim (defaultPlayer, player);
					AddCnsList (defaultPlayer, player);
					AttachPals (defaultPlayer, player);
					#endif
					player.CreatePlayerDisplay ();
				}
			}
		}
	}
}
