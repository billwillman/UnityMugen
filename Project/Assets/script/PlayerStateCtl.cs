using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Mugen;

public class PlayerStateCtl: MonoBehaviour, IState<PlayerState, PlayerStateMgr>
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

    }
    public virtual void Exit(PlayerStateMgr target)
    {

    }

    public virtual void Process(PlayerStateMgr target)
    {

    }

    public PlayerState Id
    {
        get;
        set;
    }

    // 注冊角色狀態
    public static void RegisterPlayerStates()
    {

    }
}
