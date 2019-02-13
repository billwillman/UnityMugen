﻿using UnityEngine;
using System.Collections;
using Utils;
using Mugen;

[RequireComponent(typeof(PlayerDisplay))]
public class PlayerStateMgr : MonoBehaviour {
    private StateMgr<PlayerState, PlayerStateMgr> m_StateMgr = null;
	private PlayerDisplay m_PlayerDisplay = null;
    private bool m_HasFirstChangedStated = false;
    private bool m_IsCnsState = false;

    public void SetCnsState(bool isUseCns)
    {
        m_IsCnsState = isUseCns;
    }
    void Awake()
    {
        m_StateMgr = new StateMgr<PlayerState, PlayerStateMgr>(this);
    }

	public bool CanUseCnsCtl
	{
		get {
			var player = this.PlyDisplay;
			if (player == null)
				return false;
			return player.HasCnsFiles;
		}
	}

	public bool ChangeState(PlayerState state, bool isCns  = false)
    {
        if (m_StateMgr == null)
            return false;
		var display = this.PlyDisplay;
        if (display == null)
            return false;
		if (display.PlyType == InputPlayerType.none)
			return false;
        if (m_StateMgr.CurrStateKey == state && m_HasFirstChangedStated)
            return false;
        if (!m_HasFirstChangedStated)
            m_HasFirstChangedStated = true;
		if (!isCns) {
            if (!display.HasBeginActionSrpiteData(state, true))
				return false;
		} else {
			var player = display.GPlayer;
            if (player == null)
                return false;
            var cnsCfg = player.CnsCfg;
            if (cnsCfg == null)
            {
                if (player.LuaCfg != null)
                    cnsCfg = player.LuaCfg.CnsCfg;
            }
            if (cnsCfg == null || !cnsCfg.HasStateDef)
				return false;
            var def = cnsCfg.GetStateDef((int)state);
			if (def == null)
				return false;
            if (!display.HasBeginActionSrpiteData((PlayerState)def.Anim))
				return false;
		}

        m_IsCnsState = isCns;
        return m_StateMgr.ChangeState(state, m_IsCnsState);
    }

	public PlayerDisplay PlyDisplay
	{
		get {
			if (m_PlayerDisplay == null)
				m_PlayerDisplay = GetComponent<PlayerDisplay> ();
			return m_PlayerDisplay;
		}
	}

	public PlayerState CurState
	{
		get
		{
			if (m_StateMgr == null)
				return PlayerState.psNone;
			return m_StateMgr.CurrStateKey;
		}
	}

	public void CurStateOnAnimateEndFrame()
	{
		if (m_StateMgr == null)
			return;

		bool isDone = false;
		var listener = m_StateMgr.Listener as CnsPlayerStateCtl;
		if (listener != null) {
			listener.OnAnimateEndFrame (this, ref isDone);
		}

		if (!isDone) {
			var state = m_StateMgr.CurrState as BasePlayerState;
			if (state == null)
				return;
			state.OnAnimateEndFrame (this);
		}
	}

	void LateUpdate()
	{
		if (m_StateMgr != null)
            m_StateMgr.Process(this, m_IsCnsState);
	}
}
