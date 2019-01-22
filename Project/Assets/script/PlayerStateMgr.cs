using UnityEngine;
using System.Collections;
using Utils;
using Mugen;

public class PlayerStateMgr : MonoBehaviour {
    private StateMgr<PlayerState, PlayerStateMgr> m_StateMgr = null;

    void Awake()
    {
        m_StateMgr = new StateMgr<PlayerState, PlayerStateMgr>(this);
    }

    public bool ChangeState(PlayerState state)
    {
        if (m_StateMgr == null)
            return false;
        return m_StateMgr.ChangeState(state);
    }
	
}
