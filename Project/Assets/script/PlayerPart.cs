using UnityEngine;
using System.Collections;
using Mugen;

[RequireComponent(typeof(PlayerDisplay))]
public class PlayerPart : MonoBehaviour {
	private PlayerDisplay m_Display = null;

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
		if (display != null)
			display.InteralRefreshCurFrame (target);
	}
}
