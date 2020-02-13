using UnityEngine;
using System.Collections;
using Utils;

public enum CameraFollowMode
{
	none,
	_1p,
	_2p,
	_3p,
	_4p
}

[RequireComponent(typeof(Camera))]
public class CameraFollow : CachedMonoBehaviour {

	public float m_SmoothTime = 0.25f;
	public Vector3 m_Vel = Vector3.zero;

	protected PlayerDisplay Target
	{
		get
		{
			switch (AppConfig.GetInstance ().m_CameraFollowTarget) {
			case CameraFollowMode._1p:
				return PlayerControls.GetInstance ().GetPlayer (InputPlayerType._1p);
			case CameraFollowMode._2p:
				return PlayerControls.GetInstance ().GetPlayer (InputPlayerType._2p);
			case CameraFollowMode._3p:
				return PlayerControls.GetInstance ().GetPlayer (InputPlayerType._3p);
			case CameraFollowMode._4p:
				return PlayerControls.GetInstance ().GetPlayer (InputPlayerType._4p);
			}
			return null;
		}
	}

	void LateUpdate()
	{
		
		var target = this.Target;
		if (target != null) {
			var trans = this.CachedTransform;
			var srcPt = trans.position;
			var targetPt = target.CachedTransform.position;
			targetPt.y = srcPt.y;
			targetPt.z = srcPt.z;
			trans.position = Vector3.SmoothDamp (trans.position, targetPt, ref m_Vel, m_SmoothTime); 
		}
	}
}
