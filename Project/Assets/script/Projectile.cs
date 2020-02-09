using UnityEngine;
using System.Collections;
using Mugen;

[RequireComponent(typeof(PlayerDisplay))]
public class Projectile : MonoBehaviour {

	private PlayerDisplay m_Display = null;

	protected PlayerDisplay Display
	{
		get {
			if (m_Display == null)
				m_Display = GetComponent<PlayerDisplay> ();
			return m_Display;
		}
	}

	public int projid = -1;
	public int projanim = (int)PlayerState.psNone;
	public int projhitanim = (int)PlayerState.psNone;
	public int projremove = 1;
	public int projcancelanim = (int)PlayerState.psNone;
	public int projremovetime = -1;
	public int offset_x = 0;
	public int offset_y = 0;
	public float velocity_x = 0;
	public float velocity_y = 0;
	public ExplodPosType Postype = ExplodPosType.p1;

	public void Apply()
	{
		var display = this.Display;
		if (display != null) {
			display.PlayAni ((PlayerState)projanim, true);
		}
	}
}
