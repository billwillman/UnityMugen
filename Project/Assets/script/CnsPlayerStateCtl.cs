using UnityEngine;
using System.Collections;
using Utils;
using Mugen;

public class CnsPlayerStateCtl : MonoBehaviour, IStateMgrListener<PlayerState, PlayerStateMgr> {


	public virtual void Init()
	{
		StateMgr<PlayerState, PlayerStateMgr>.SetListener (this);
	}

	public virtual bool CanEnter(PlayerStateMgr target, PlayerState id, PlayerState newId, ref bool isDone)
	{
		if (!target.CanUseCnsCtl)
			return true;

		isDone = true;

		return true;
	}

	public virtual bool CanExit(PlayerStateMgr target, PlayerState id, ref bool isDone)
	{
		if (!target.CanUseCnsCtl)
			return true;
		
		isDone = true;
		return true;
	}
	public virtual void Enter(PlayerStateMgr target, PlayerState id)
	{
		var player = target.PlyDisplay;
		if (player == null)
			return;

		if (player.PlayCnsAnimate ((int)id))
			return;

		var plyState = target.CurState;
		switch (plyState) {
		case PlayerState.psStand1:
			player.PlayAni (plyState);
			break;
		}
	}

	public virtual void Exit(PlayerStateMgr target, PlayerState id)
	{
	}

	public virtual void Process(PlayerStateMgr target, PlayerState id, ref bool isDone)
	{
		if (!target.CanUseCnsCtl)
			return;
		isDone = true;
	}

	public virtual void OnAnimateEndFrame (PlayerStateMgr target, ref bool isDone)
	{
		if (!target.CanUseCnsCtl)
			return;
		
		isDone = true;
	}


}
