using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;

public class SpriteMovement : MonoBehaviour {

	// 开始速度
	public Vector2 StartVec  = Vector2.zero;
	public Vector2 Vec = Vector2.zero;

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
			m_Display.m_OffsetPos = value;
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

	void Update()
	{
		float deltaTime = Time.deltaTime;
		if (!UpdatePause (deltaTime))
			UpdateMove (deltaTime);
	}
		

	void UpdateMove(float deltaTime)
	{
		if (Mathf.Abs (Vec.x) <= float.Epsilon && Mathf.Abs (Vec.y) <= float.Epsilon)
			return;
		// 按照毫秒算速度
		Vector2 vv = Vec * (IsFlipX? -1:1) * deltaTime * 1000f;
		Vector2 org = this.OffsetPos;
		org += vv;
		this.OffsetPos = org;
	}

	public void AniCtlPause(float pauseTime)
	{
		this.AniCtlPauseTime = pauseTime;
	}
}
