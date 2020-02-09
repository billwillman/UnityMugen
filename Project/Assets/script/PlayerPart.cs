using UnityEngine;
using System.Collections;
using LuaInterface;
using Mugen;

[RequireComponent(typeof(PlayerDisplay))]
public class PlayerPart : MonoBehaviour {
	
	private PlayerDisplay m_Display = null;
	public ExplodPosType postype = ExplodPosType.p1;
	/*
	public PlayerState m_State = PlayerState.psNone;
*/
	protected PlayerDisplay Display {
		get {
			if (m_Display == null)
				m_Display = GetComponent<PlayerDisplay> ();
			return m_Display;
		}
	}

	protected void CheckVisible()
	{
		var display = this.Display;
		if (display != null)
			display.ResetPlayerPart ();
	}

	/*
	void Update()
	{
		CheckVisible ();
	}*/

	protected bool IsDestroying
	{
		get {
			var display = this.Display;
			if (display == null)
				return false;
			return display.IsDestroying;
		}
	}

	protected PlayerDisplay GetParentDisplay()
	{
		if (IsDestroying)
			return null;
		
		switch (postype) {
		case ExplodPosType.p1:
			return PlayerControls.GetInstance ().GetPlayer (InputPlayerType._1p);
		case ExplodPosType.p2:
			return PlayerControls.GetInstance ().GetPlayer (InputPlayerType._2p);
		default:
			return null;
		}
	}

	protected void ApplyParentDisplayLoaderPlayer(PlayerDisplay parent)
	{
		var display = this.Display;
		if (display == null)
			return;
		if (parent == null)
			parent = GetParentDisplay ();
		if (parent != null) {
			display._SetLoaderPlayer (parent.LoaderPlayer);
		}
	}

	protected void InternalParentUpdateFrame(ImageAnimation target, PlayerState state)
	{
		var display = this.Display;
		if (display != null) {
			CheckVisible ();

			var targetDisplay = target.CacheDisplayer;
			if (targetDisplay == null)
				return;
			var image = display.ImageAni;
			if (image != null) {
				display._SetLoaderPlayer (targetDisplay.LoaderPlayer);
				image._SetStateFrameList (state, target.CurFrame);
				display.InteralRefreshCurFrame (image);
				//SendMessage ("OnPlayerPartUpdateFrame", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	[NoToLua]
	internal virtual void OnParentUpdateFrame(ImageAnimation target)
	{
	}

	[NoToLua]
	internal virtual void OnParentFrameEnd(ImageAnimation target)
	{
	}

	[NoToLua]
	internal virtual void OnParentFramePosUpdate(ImageAnimation target)
	{
	}

	internal void InteralDestroy()
	{
		var display = GetParentDisplay ();
		if (display != null)
			display._OnPlayerPartDestroy (this);
	}

	void OnDestroy()
	{
		if (!AppConfig.IsAppQuit) {
			InteralDestroy ();
		}
	}
}
