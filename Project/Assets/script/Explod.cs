using UnityEngine;
using System.Collections;
using LuaInterface;
using Mugen;

public enum ExplodPosType
{
	p1,
	p2,
	back,
	left,
	right
}

[RequireComponent(typeof(PlayerDisplay))]
public class Explod : PlayerPart {
	private Vector2 m_StartOffset = Vector2.zero;

	public int anim = (int)PlayerState.psNone;
	public int ID = -1;
	public int removetime = -2;
	public float bindtime = -1; // 单位：秒
	public int pos_x = 0;
	public int pos_y = 0;
	public float x_vel;
	public float y_vel;
	public int sprpriority = 0;
	public int removeongethit = 0;
	public int ignorehitpause = 1;
	public bool IsUseParentUpdate = true;
	public Vector2 scale = Vector2.one;

	void Awake()
	{
		this.ShowType = DisplayType.Explod;
	}

	void InitOffsetPos(PlayerDisplay parentDisplay)
	{
		if (IsDestroying || parentDisplay == null)
			return;
		
		if (postype == ExplodPosType.p1) {
			var display = this.Display;
			if (display != null) 
			{
				Vector2 offset =  parentDisplay.m_OffsetPos;
				Vector2 v = new Vector2((float)pos_x, (float)pos_y);
				if (parentDisplay.IsFlipX)
					v.x = -v.x;
				Vector2 vv = v / 100f + offset;
				display.m_OffsetPos = vv;
				display.m_OffsetPos.z = -sprpriority;
				display.IsFlipX = parentDisplay.IsFlipX;
			}
		}
	}

	private void FollowParent()
	{
		var parentDisplay = GetParentDisplay ();
		InitOffsetPos (parentDisplay);
	}

	public void Apply()
	{
		if (IsDestroying)
			return;
		
		var display = this.Display;
		if (display != null) {
			var parentDisplay = GetParentDisplay ();
			InitOffsetPos (parentDisplay);
			display.SetVelSet (x_vel, y_vel);
			var localScale = display.CachedTransform.localScale;
			localScale.x = scale.x;
			localScale.y = scale.y;
			display.CachedTransform.localScale = localScale;
			if (!IsUseParentUpdate) {
				bool isLoop = removetime == -1;
				ApplyParentDisplayLoaderPlayer (parentDisplay);
				display.PlayAni (anim, isLoop);
			}
		}
	}

	[NoToLua]
	internal override void OnParentUpdateFrame(ImageAnimation target)
	{
		if (IsDestroying)
			return;


		if (IsUseParentUpdate) {
			this.InternalParentUpdateFrame (target, (PlayerState)anim);
		}
	}

	[NoToLua]
	internal override void OnParentFrameEnd(ImageAnimation target)
	{
		if (IsUseParentUpdate) {
			if (removetime == -2) {
				InteralDoDestroy ();
			}
		}
	}

	[NoToLua]
	internal override void OnParentFramePosUpdate(ImageAnimation target)
	{
		if (IsDestroying)
			return;
	}

	private void CheckSelfRemoveTime()
	{
		if (!IsUseParentUpdate) {
			var display = this.Display;
			if (display != null) {
				if (display.Trigger_AnimTime () == 0) {
					// 最后一帧
					if (removetime == -2) {
						InteralDoDestroy ();
					}
				}
			}
		}
	}

	void OnImageAnimationEndFrame()
	{
		CheckSelfRemoveTime ();
	}

	void OnImageAnimationFrame()
	{
		CheckSelfRemoveTime ();
	}

	/*
	void OnImageAniTimeUpdate()
	{
		var display = this.Display;
		if (display != null) {
			var ani = display.ImageAni;
			if (ani != null && ani.ChekAnimCntOne()) {
				CheckSelfRemoveTime ();
			}
		}
	}*/

	void UpdateBindTime()
	{
		if (bindtime > 0) {
			bindtime -= AppConfig.GetInstance ().DeltaTick;
			if (bindtime <= 0) {
				bindtime = -1;
				//	InteralDoDestroy ();
			} else {
				// 跟随角色
				FollowParent();
			}
		}

		//FollowParent ();
	}

	void UpdateRemoveTime()
	{
		if (removetime > 0) {
			--removetime;
			if (removetime <= 0) {
				InteralDoDestroy ();
			}
		}
	}

	void LateUpdate()
	{
		if (!AppConfig.GetInstance ().IsUsePhysixUpdate) {
			UpdateBindTime ();
			UpdateRemoveTime ();
		}
	}

	void FixedUpdate()
	{
		if (AppConfig.GetInstance ().IsUsePhysixUpdate) {
			UpdateBindTime ();
			UpdateRemoveTime ();
		}
	}

}
