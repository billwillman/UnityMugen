using UnityEngine;
using System.Collections;
using Utils;
using Mugen;

[RequireComponent(typeof(PlayerDisplay))]
public class PlayerStateMgr : MonoBehaviour {
    private StateMgr<PlayerState, PlayerStateMgr> m_StateMgr = null;
	private PlayerDisplay m_PlayerDisplay = null;

    void Awake()
    {
        m_StateMgr = new StateMgr<PlayerState, PlayerStateMgr>(this);
    }

    public bool ChangeState(PlayerState state)
    {
        if (m_StateMgr == null)
            return false;
		var display = this.PlyDisplay;
		if (display.PlyType == InputPlayerType.none)
			return false;
        if (m_StateMgr.CurrStateKey == state)
            return false;
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
		var state = m_StateMgr.CurrState as BasePlayerState;
		if (state == null)
			return;
		state.OnAnimateEndFrame (this);
	}

	void LateUpdate()
	{
		if (m_StateMgr != null)
			m_StateMgr.Process (this);
	}
}
