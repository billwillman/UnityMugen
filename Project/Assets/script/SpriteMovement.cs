using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;

public class SpriteMovement : MonoBehaviour {

	// 开始速度
	public Vector2 StartVec  = Vector2.zero;
	public Vector2 Vec = Vector2.zero;
	public float g = 9.8f;

	public float AniCtlPauseTime = -1;

	private ImageAnimation m_Animation = null;
	private PlayerDisplay m_Display = null;

	void Start()
	{
		m_Animation = GetComponent<ImageAnimation> ();
		m_Display = GetComponent<PlayerDisplay> ();
	}

	protected ImageAnimation CacheAnimation
	{
		get
		{
			return m_Animation;
		}
	}

	protected bool IsFlipX
	{
		get {
			if (m_Display == null)
				return false;
			return m_Display.IsFlipX;
		}
	}

	protected Vector2 OffsetPos
	{
		get
		{
			if (m_Display == null)
				return Vector2.zero;
			return m_Display.m_OffsetPos;
		}

		set
		{
			if (m_Display == null)
				return;
			float z = m_Display.m_OffsetPos.z;
			//Debug.LogError (z.ToString ());
			m_Display.m_OffsetPos = value;
			m_Display.m_OffsetPos.z = z;
		}
	}

	bool UpdatePause(float deltaTime)
	{
		if (AniCtlPauseTime <= 0)
			return false;
		if (AniCtlPauseTime - deltaTime <= 0) {
			AniCtlPauseTime = -1;
			var ani = this.CacheAnimation;
			if (ani != null) {
				ani.Pause (false);
			}
			return false;
		} else {
			AniCtlPauseTime -= deltaTime;
			return true;
		}
	}

	void FixedUpdate()
	{
		if (AppConfig.GetInstance ().IsUsePhysixUpdate)
			MoveUpdate ();
	}

	void MoveUpdate()
	{
		float deltaTime = AppConfig.GetInstance().DeltaTime;
		if (!UpdatePause (deltaTime))
			UpdateMove (deltaTime);
	}

	void Update()
	{
		if (!AppConfig.GetInstance ().IsUsePhysixUpdate)
			MoveUpdate ();
	}
		

	void UpdateMove(float deltaTime)
	{
		if (Mathf.Abs (Vec.x) <= float.Epsilon && Mathf.Abs (Vec.y) <= float.Epsilon)
			return;
		// 按照毫秒算速度
		float d = deltaTime * 1000f;
      //  if (m_Display != null && m_Display.Attribe.PhysicsType == Cns_PhysicsType.A) {
            float gg = -g / (PlayerDisplay._cVelPerUnit * PlayerDisplay._cAPerUnit);
            //float gg = -g/1000000f * 6.5f;
            Vec.y += gg * d;
        //  }

        Vector2 vv = new Vector2(Vec.x * (IsFlipX? -1:1), Vec.y);

        if (Mathf.Abs(vv.x) > float.Epsilon) {
            float oldVx = vv.x;
            // 动态摩擦因子
            float u = AppConfig.GetInstance().u * StageMgr.GetInstance().u;
            // 摩擦力的向下地面fn
            float aX = u * g;
            if (vv.x > 0)
                aX = -aX;
            vv.x = aX * deltaTime + vv.x;
            if (oldVx * vv.x < 0)
                vv.x = 0;
        }

        Vector2 org = this.OffsetPos;
		org += vv * d;
		if (org.y < 0) {
			//Vec.y = 0;
			org.y = 0;
		}
		this.OffsetPos = org;
		if (m_Display != null) {
			m_Display.InternalUpdatePos ();
			//Debug.LogError (m_Display.transform.localPosition.z.ToString ());
		}
	}

	public void AniCtlPause(float pauseTime)
	{
		this.AniCtlPauseTime = pauseTime;
	}
}
