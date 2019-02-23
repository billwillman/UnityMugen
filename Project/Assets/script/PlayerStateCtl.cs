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

    private void DefaultEnter(PlayerStateMgr target)
    {
        var player = target.PlyDisplay;
        if (player == null)
            return;
        var plyState = target.CurState;
		player.PlayAni(plyState);
		/*
        switch (plyState)
        {
            case PlayerState.psStand1:
                player.PlayAni(plyState);
                break;
            case PlayerState.psDown1:
                player.PlayAni(PlayerState.psDown1, false);
                break;
            case PlayerState.psBackWalk1:
                player.PlayAni(PlayerState.psBackWalk1);
                break;
            case PlayerState.psForwardWalk1:
                player.PlayAni(PlayerState.psForwardWalk1);
                break;
            case PlayerState.psForwardRun1:
                player.PlayAni(PlayerState.psForwardRun1);
                break;
            case PlayerState.psBackStep1:
                player.PlayAni(PlayerState.psBackStep1, false);
                break;
            case (PlayerState)210:
                player.PlayAni((PlayerState)210, false);
                break;
            case (PlayerState)200:
                player.PlayAni((PlayerState)200, false);
                break;
            case (PlayerState)320:
                player.PlayAni((PlayerState)320, false);
                break;
            case (PlayerState)400:
                player.PlayAni((PlayerState)400, false);
                break;
            case (PlayerState)410:
                player.PlayAni((PlayerState)410, false);
                break;
            case (PlayerState)420:
                player.PlayAni((PlayerState)410, false);
                break;
            case (PlayerState)430:
                player.PlayAni((PlayerState)430, false);
                break;
        }
        */
    }

	public virtual void Enter(PlayerStateMgr target)
	{
        DefaultEnter(target);
	}
	public virtual void Exit(PlayerStateMgr target)
	{
	}

	private InputControlType CompareLeftOrRight(PlayerStateMgr target, InputControlType ctlType)
	{
		if (target.PlyDisplay.IsFlipX) {
			if (ctlType == InputControlType.left)
				return InputControlType.right;
			else if (ctlType == InputControlType.right)
				return InputControlType.left;
		}

		return ctlType;
	}

	private void CheckNoAttackProcess(PlayerStateMgr target)
	{
		var player = target.PlyDisplay;
		if (player == null)
			return;

		var plyType = player.PlyType;
		if (plyType == InputPlayerType.none)
			return;

		int runValue = PlayerControls.GetInstance ().InputCtl.GetPlayerRunKeyValue (player.PlyType);

        if (runValue == 0 && !PlayerControls.GetInstance().InputCtl.IsVaildCanPressKeyPress(plyType))
			target.ChangeState (PlayerState.psStand1);

		if ((runValue & (int)InputControlType.down) != 0)
			target.ChangeState (PlayerState.psDown1);
		else if (runValue == (int)CompareLeftOrRight(target, InputControlType.left)) {
			var inputList = PlayerControls.GetInstance ().InputCtl.GetInputList (plyType);
			if (inputList != null && inputList.Count >= 2) {
				var i1 = inputList [inputList.Count - 2];
				var i2 = inputList [inputList.Count - 1];

				if (i1.keyCodeValue == i2.keyCodeValue && i1.keyCodeValue == (int)CompareLeftOrRight(target, InputControlType.left)) {
					float delta = i2.downTick - i1.downTick;
					if (/*delta >= 0.16f &&*/ delta <= 0.22f) {
						target.ChangeState (PlayerState.psBackStep1);
						//	Debug.LogError (delta.ToString ());
						return;
					}

				}
			}
			if (target.CurState != PlayerState.psBackStep1)
				target.ChangeState (PlayerState.psBackWalk1);
		} else if (runValue == (int)CompareLeftOrRight(target, InputControlType.right)) {
			var inputList = PlayerControls.GetInstance ().InputCtl.GetInputList (plyType);
			if (inputList != null && inputList.Count >= 2) {
				var i1 = inputList [inputList.Count - 2];
				var i2 = inputList [inputList.Count - 1];
				if (i1.keyCodeValue == i2.keyCodeValue && i1.keyCodeValue == (int)CompareLeftOrRight(target, InputControlType.right)) {
					float delta = i2.downTick - i1.downTick;
					if (delta >= 0.16f && delta <= 0.22f) {
						target.ChangeState (PlayerState.psForwardRun1);
						return;

					}
				}
			}
			if (target.CurState != PlayerState.psForwardRun1)
				target.ChangeState (PlayerState.psForwardWalk1);
		} else if (runValue == (int)InputControlType.attack1) {
			if (IsDownPress(target))
				target.ChangeState ((PlayerState)410);
			else
				target.ChangeState ((PlayerState)210);
		} else if (runValue == (int)InputControlType.attack2) {
			if (IsDownPress(target))
				target.ChangeState ((PlayerState)400);
			else
				target.ChangeState ((PlayerState)200);
		} else if (runValue == (int)InputControlType.attack4) {
			if (IsDownPress (target))
				target.ChangeState ((PlayerState)430);
			else
				target.ChangeState ((PlayerState)320);
		} else if (runValue == (int)InputControlType.attack5) {
			if (IsDownPress (target))
				target.ChangeState ((PlayerState)420);
		}
	}

	private bool IsDownPress(PlayerStateMgr target)
	{
        return IsPress(target, InputControlType.down);
	}

    private bool IsPress(PlayerStateMgr target, InputControlType ctlType)
    {
        if (target == null || ctlType == InputControlType.none)
            return false;
        var player = target.PlyDisplay;
		if (player == null)
			return false;

		var plyType = player.PlyType;
		if (plyType == InputPlayerType.none)
			return false;
        return PlayerControls.GetInstance().InputCtl.GetKeyPress(player.PlyType, ctlType);
    }

	public virtual void Process(PlayerStateMgr target)
	{
		var player = target.PlyDisplay;
		if (player == null)
			return;

		var plyType = player.PlyType;
		if (plyType == InputPlayerType.none)
			return;

		if (IsAttackState (target))
			return;

		CheckNoAttackProcess (target);
	}

	public virtual void OnAnimateEndFrame (PlayerStateMgr target)
	{
		var player = target.PlyDisplay;
		if (player == null)
			return;

		var plyType = player.PlyType;
		if (plyType == InputPlayerType.none)
			return;

		if (IsAttackState(target)) {
			//CheckNoAttackProcess (target);
			if (IsDownPress(target))
				target.ChangeState(PlayerState.psDown1);
			else if (IsPress(target, CompareLeftOrRight(target, InputControlType.left)))
                target.ChangeState(PlayerState.psForwardWalk1);
			else if (IsPress(target, CompareLeftOrRight(target, InputControlType.right)))
                target.ChangeState(PlayerState.psBackWalk1);
            else
				target.ChangeState(PlayerState.psStand1);
		}
	}

	private bool IsAttackState(PlayerStateMgr target)
	{
		int curstate = (int)target.CurState;
		return (curstate == 210 || curstate == 200 || curstate == 320 || curstate == 400 || curstate == 410 ||curstate == 420 || curstate == 430);
	}

    private void RegisterState(PlayerState state)
    {
        StateMgr<PlayerState, PlayerStateMgr>.Register(state, new BasePlayerState(this));
    }

    public void RegisterState(int state)
    {
        RegisterState((PlayerState)state);
    }


    // 注冊角色狀態
    public void RegisterDefaultPlayerStates()
    {
        RegisterState(PlayerState.psStand1);
        RegisterState(PlayerState.psDown1);
        RegisterState(PlayerState.psBackWalk1);
        RegisterState(PlayerState.psForwardWalk1);
        RegisterState(PlayerState.psForwardRun1);
        RegisterState(PlayerState.psBackStep1);
        RegisterState(210);
        RegisterState(200);
        RegisterState(320);
        RegisterState(400);
        RegisterState(410);
        RegisterState(420);
        RegisterState(430);
    }
}
