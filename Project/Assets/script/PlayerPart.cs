using UnityEngine;
using System.Collections;
using Mugen;

[RequireComponent(typeof(PlayerDisplay))]
public class PlayerPart : MonoBehaviour {
	private PlayerDisplay m_Display = null;

	public PlayerState m_State = PlayerState.psNone;

	protected PlayerDisplay Display {
		get {
			if (m_Display == null)
				m_Display = GetComponent<PlayerDisplay> ();
			return m_Display;
		}
	}

	private void CheckVisible()
	{
		var display = this.Display;
		if (display != null)
			display.ResetPlayerPart ();
	}

	void Update()
	{
		CheckVisible ();
	}

	public void OnUpdateFrame(ImageAnimation target)
	{
		var display = this.Display;
		if (display != null) {
			var targetDisplay = target.CacheDisplayer;
			if (targetDisplay == null)
				return;
			var image = display.ImageAni;
			if (image != null) {
				display._SetLoaderPlayer (targetDisplay);
				image._SetStateFrameList (m_State, target.CurFrame);
				display.InteralRefreshCurFrame (image);
			}
		}
	}
}
