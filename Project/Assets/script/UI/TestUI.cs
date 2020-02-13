using UnityEngine;
using System.Collections;

public class TestUI : MonoBehaviour {

	public GameObject m_BtnLeft = null;
	public GameObject m_BtnRight = null;
	public GameObject m_BtnSkill1 = null;
	public GameObject m_BtnSkill2 = null;

	void Start()
	{
		if (m_BtnLeft != null) {
			UIEventListener.Get (m_BtnLeft).onPress = OnBtnLeftClick;
		}

		if (m_BtnRight != null)
			UIEventListener.Get (m_BtnRight).onPress = OnBtnRightClick;

		if (m_BtnSkill1 != null)
			UIEventListener.Get (m_BtnSkill1).onPress = OnBtnSkill1Click;

		if (m_BtnSkill2 != null)
			UIEventListener.Get (m_BtnSkill2).onPress = OnBtnSkill2Click;
	}

	void OnBtnSkill1Click(GameObject target, bool isState)
	{
		var player = PlayerControls.GetInstance ().GetPlayer (InputPlayerType._1p);
		if (player != null && player.Attribe.Ctrl != 0) {
			var gplayer = player.GPlayer;
			if (gplayer != null && gplayer.CmdCfg != null) {
				var cmd = gplayer.CmdCfg.GetCommand ("升龙");
				if (cmd != null)
					cmd.isEditorActive = isState;
			}
		}
	}

	void OnBtnSkill2Click(GameObject target, bool isState)
	{
		var player = PlayerControls.GetInstance ().GetPlayer (InputPlayerType._1p);
		if (player != null && player.Attribe.Ctrl != 0) {
			var gplayer = player.GPlayer;
			if (gplayer != null && gplayer.CmdCfg != null) {
				var cmd = gplayer.CmdCfg.GetCommand ("气功波");
				if (cmd != null)
					cmd.isEditorActive = isState;
			}
		}
	}


	public void OnBtnLeftClick(GameObject target, bool isState)
	{
		var player = PlayerControls.GetInstance ().GetPlayer (InputPlayerType._1p);
		if (player != null && player.Attribe.Ctrl != 0) {
			player.IsFlipX = false;
			var gplayer = player.GPlayer;
			if (gplayer != null && gplayer.CmdCfg != null) {
				var cmd = gplayer.CmdCfg.GetCommand ("FF");
				if (cmd != null)
					cmd.isEditorActive = isState;
			}
		}
	}

	public void OnBtnRightClick(GameObject target, bool isState)
	{
		var player = PlayerControls.GetInstance ().GetPlayer (InputPlayerType._1p);
		if (player != null && player.Attribe.Ctrl != 0) {
			player.IsFlipX = true;
			var gplayer = player.GPlayer;
			if (gplayer != null && gplayer.CmdCfg != null) {
				var cmd = gplayer.CmdCfg.GetCommand ("FF");
				if (cmd != null)
					cmd.isEditorActive = isState;
			}
		}
	}
}
