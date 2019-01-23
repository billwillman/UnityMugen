using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Mugen;


public interface IBasePlayerStateListener
{
	bool CanEnter (PlayerStateMgr target);
	bool CanExit (PlayerStateMgr target);
	void Enter (PlayerStateMgr target);
	void Exit(PlayerStateMgr target);
	void Process(PlayerStateMgr target);
	void OnAnimateEndFrame (PlayerStateMgr target);
}

public class BasePlayerState: IState<PlayerState, PlayerStateMgr>
{
	private IBasePlayerStateListener m_Listener = null;
	public BasePlayerState(IBasePlayerStateListener listener)
	{
		m_Listener = listener;
	}


	public virtual bool CanEnter(PlayerStateMgr target)
	{
		if (m_Listener != null)
			return m_Listener.CanEnter (target);
		return false;
	}

	public virtual bool CanExit(PlayerStateMgr target)
	{
		if (m_Listener != null)
			return m_Listener.CanExit (target);
		return false;
	}

	public virtual void Enter(PlayerStateMgr target)
	{
		if (m_Listener != null)
			m_Listener.Enter (target);
	}
	public virtual void Exit(PlayerStateMgr target)
	{
		if (m_Listener != null)
			m_Listener.Exit (target);
	}

	public virtual void Process(PlayerStateMgr target)
	{
		if (m_Listener != null)
			m_Listener.Process (target);
	}

	public virtual void OnAnimateEndFrame(PlayerStateMgr target)
	{
		if (m_Listener != null)
			m_Listener.OnAnimateEndFrame (target);
	}

	public PlayerState Id
	{
		get;
		set;
	}
}

public class PlayerStateCtl: MonoBehaviour, IBasePlayerStateListener
{
    
	public virtual bool CanEnter(PlayerStateMgr target)
	{
		return true;
	}

	public virtual bool CanExit(PlayerStateMgr target)
	{
		return true;
	}

	public virtual void Enter(PlayerStateMgr target)
	{
		var player = target.PlyDisplay;
		if (player == null)
			return;
		var plyState = target.CurState;
		switch (plyState) {
		case PlayerState.psStand1:
			player.PlayAni (plyState);
			break;
		case PlayerState.psDown1:
			player.PlayAni (PlayerState.psDown1, false);
			break;
		case PlayerState.psBackWalk1:
			player.PlayAni (PlayerState.psBackWalk1);
			break;
		case PlayerState.psForwardWalk1:
			player.PlayAni (PlayerState.psForwardWalk1);
			break;
		case PlayerState.psForwardRun1:
			player.PlayAni (PlayerState.psForwardRun1);
			break;
		case PlayerState.psBackStep1:
			player.PlayAni (PlayerState.psBackStep1, false);
			break;
		case (PlayerState)200:
			player.PlayAni ((PlayerState)200, false);
			break;
		}
	}
	public virtual void Exit(PlayerStateMgr target)
	{
	}

	public virtual void Process(PlayerStateMgr target)
	{
		var player = target.PlyDisplay;
		if (player == null)
			return;

		var plyType = player.PlyType;
		if (plyType == InputPlayerType.none)
			return;

		int runValue = PlayerControls.GetInstance ().InputCtl.GetPlayerRunKeyValue (player.PlyType);

		if (runValue == 0 && target.CurState != (PlayerState)200)
			target.ChangeState (PlayerState.psStand1);

		if ((runValue & (int)InputControlType.down) != 0)
			target.ChangeState (PlayerState.psDown1);
		else if (runValue == (int)InputControlType.left) {
			var inputList = PlayerControls.GetInstance ().InputCtl.GetInputList (plyType);
			if (inputList != null && inputList.Count >= 2) {
				var i1 = inputList [inputList.Count - 2];
				var i2 = inputList [inputList.Count - 1];

				if (i1.keyCodeValue == i2.keyCodeValue && i1.keyCodeValue == (int)InputControlType.left) {
					float delta = i2.downTick - i1.downTick;
					if (delta >= 0.16f && delta <= 0.22f) {
						target.ChangeState (PlayerState.psBackStep1);
						//	Debug.LogError (delta.ToString ());
						return;
					}

				}
			}
			if (target.CurState != PlayerState.psBackStep1)
				target.ChangeState (PlayerState.psBackWalk1);
		} else if (runValue == (int)InputControlType.right) {
			var inputList = PlayerControls.GetInstance ().InputCtl.GetInputList (plyType);
			if (inputList != null && inputList.Count >= 2) {
				var i1 = inputList [inputList.Count - 2];
				var i2 = inputList [inputList.Count - 1];
				if (i1.keyCodeValue == i2.keyCodeValue && i1.keyCodeValue == (int)InputControlType.right) {
					float delta = i2.downTick - i1.downTick;
					//if (delta >= 0.16f && delta <= 0.22f) {
						target.ChangeState (PlayerState.psForwardRun1);
						return;

					//}
				}
			}
			if (target.CurState != PlayerState.psForwardRun1)
				target.ChangeState (PlayerState.psForwardWalk1);
		} else if (runValue == (int)InputControlType.attack1) {
			target.ChangeState ((PlayerState)200);
		}
	}

	public virtual void OnAnimateEndFrame (PlayerStateMgr target)
	{
		var player = target.PlyDisplay;
		if (player == null)
			return;

		var plyType = player.PlyType;
		if (plyType == InputPlayerType.none)
			return;

		if (target.CurState == (PlayerState)200) {
			target.ChangeState (PlayerState.psStand1);
		}
	}


    // 注冊角色狀態
    public void RegisterDefaultPlayerStates()
    {
		StateMgr<PlayerState, PlayerStateMgr>.Register(PlayerState.psStand1, new BasePlayerState(this));
		StateMgr<PlayerState, PlayerStateMgr>.Register(PlayerState.psDown1, new BasePlayerState(this)); 
		StateMgr<PlayerState, PlayerStateMgr>.Register(PlayerState.psBackWalk1, new BasePlayerState(this)); 
		StateMgr<PlayerState, PlayerStateMgr>.Register(PlayerState.psForwardWalk1, new BasePlayerState(this)); 
		StateMgr<PlayerState, PlayerStateMgr>.Register(PlayerState.psForwardRun1, new BasePlayerState(this));
		StateMgr<PlayerState, PlayerStateMgr>.Register(PlayerState.psBackStep1, new BasePlayerState(this));
		StateMgr<PlayerState, PlayerStateMgr>.Register((PlayerState)200, new BasePlayerState(this));
    }
}
