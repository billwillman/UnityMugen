using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;

public class SpriteMovement : MonoBehaviour {

	// 开始速度
	public Vector2 StartVec  = Vector2.zero;

	public float AniCtlPauseTime = -1;

	private ImageAnimation m_Animation = null;
	private bool m_IsInitAnimation = false;

	protected ImageAnimation CacheAnimation
	{
		get
		{
			if (m_Animation == null && !m_IsInitAnimation)
			{
				m_IsInitAnimation = true;
				m_Animation = GetComponent<ImageAnimation> ();
			}
			return m_Animation;
		}
	}

	void UpdatePause(float deltaTime)
	{
		if (AniCtlPauseTime <= 0)
			return;
		if (AniCtlPauseTime - deltaTime <= 0) {
			AniCtlPauseTime = -1;
			var ani = this.CacheAnimation;
			if (ani != null) {
				ani.Pause (false);
			}
		} else
			AniCtlPauseTime -= deltaTime;
	}

	void Update()
	{
		float deltaTime = Time.deltaTime;
		UpdatePause (deltaTime);
	}
		

	public void AniCtlPause(float pauseTime)
	{
		this.AniCtlPauseTime = pauseTime;
	}
}
