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

	void Awake()
	{
		var display = this.Display;
		if (display != null) {
			display.ShowType = DisplayType.Projectile;
		}
	}

	public int projid = -1;
	public int projanim = (int)PlayerState.psNone;
	public int projhitanim = (int)PlayerState.psNone;
	public int projremove = 1;
	public int projcancelanim = (int)PlayerState.psNone;
	public int projremanim = (int)PlayerState.psNone;
	public float projremovetime = -1;
	public int offset_x = 0;
	public int offset_y = 0;
	public float velocity_x = 0;
	public float velocity_y = 0;
	public ExplodPosType Postype = ExplodPosType.p1;
	public int projshadow = 0;
	public int projpriority = 0;
	public int projsprpriority = 0;

	internal InputPlayerType OwnerCtl = InputPlayerType.none;

	void UpdateRemoveTime()
	{
		if (projremovetime > 0) {
			projremovetime -= AppConfig.GetInstance ().DeltaTick;
			if (projremovetime <= 0) {
				projremovetime = -1;
				var display = this.Display;
				if (display != null)
					display.DestroySelf ();
			}
		}
	}

	void LateUpdate()
	{
		if (!AppConfig.GetInstance ().IsUsePhysixUpdate)
			UpdateRemoveTime ();
	}

	void FixedUpdate()
	{
		if (AppConfig.GetInstance ().IsUsePhysixUpdate)
			UpdateRemoveTime ();
	}

	protected PlayerDisplay Owner
	{
		get
		{
			return PlayerControls.GetInstance ().GetPlayer (OwnerCtl);
		}
	}

	public void Apply()
	{
		var owner = this.Owner;
		if (owner == null)
			return;
		var display = this.Display;
		if (display != null) {
			display._SetLoaderPlayer (owner.LoaderPlayer);
			float offz = -projsprpriority;

			float offx = offset_x / PlayerDisplay._cPerUnit;
			float offy = -offset_y / PlayerDisplay._cPerUnit;
			float velx = velocity_x / PlayerDisplay._cVelPerUnit;
			float vely = -velocity_y / PlayerDisplay._cVelPerUnit;

			if (Postype == ExplodPosType.p1) {
				if (owner.IsFlipX) {
					offx = -offx;
					velx = -velx;
					//offz = -offz;
				}
			}

			display.m_OffsetPos.x = offx + owner.m_OffsetPos.x;
			display.m_OffsetPos.y = offy + owner.m_OffsetPos.y;
			display.m_OffsetPos.z = offz;
			//Debug.LogError (offz.ToString ());
			display.SetVelSet (velx, vely);
			display.PlayAni ((PlayerState)projanim, true);
		}
	}
}
