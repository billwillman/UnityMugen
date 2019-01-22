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

		int runValue = PlayerControls.GetInstance ().InputCtl.GetPlayerRunKeyValue (player.PlyType);

		if (runValue == 0)
			target.ChangeState (PlayerState.psStand1);

		if ((runValue & (int)InputControlType.down) != 0)
			target.ChangeState (PlayerState.psDown1);
		else if (runValue == (int)InputControlType.left) {
			target.ChangeState (PlayerState.psBackWalk1);
		} else if (runValue == (int)InputControlType.right) {
			target.ChangeState (PlayerState.psForwardWalk1);
		}
	}

	public virtual void OnAnimateEndFrame (PlayerStateMgr target)
	{}


    // 注冊角色狀態
    public void RegisterDefaultPlayerStates()
    {
		StateMgr<PlayerState, PlayerStateMgr>.Register(PlayerState.psStand1, new BasePlayerState(this));
		StateMgr<PlayerState, PlayerStateMgr>.Register(PlayerState.psDown1, new BasePlayerState(this)); 
		StateMgr<PlayerState, PlayerStateMgr>.Register(PlayerState.psBackWalk1, new BasePlayerState(this)); 
		StateMgr<PlayerState, PlayerStateMgr>.Register(PlayerState.psForwardWalk1, new BasePlayerState(this)); 
    }
}
