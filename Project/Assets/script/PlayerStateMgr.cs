using UnityEngine;
using System.Collections;
using Utils;
using Mugen;

[RequireComponent(typeof(PlayerDisplay))]
public class PlayerStateMgr : MonoBehaviour {
    private StateMgr<PlayerState, PlayerStateMgr> m_StateMgr = null;
	private PlayerDisplay m_PlayerDisplay = null;
    private bool m_HasFirstChangedStated = false;

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
		if (display.PlyType == InputPlayerType.none)
			return false;
        if (m_StateMgr.CurrStateKey == state && m_HasFirstChangedStated)
            return false;
        if (!m_HasFirstChangedStated)
            m_HasFirstChangedStated = true;
		if (!isCns) {
			if (!display.HasStateImage (state, true))
				return false;
		}
        return m_StateMgr.ChangeState(state);
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
			m_StateMgr.Process (this);
	}
}
