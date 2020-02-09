using UnityEngine;
using System.Collections;
using Utils;
using Mugen;

[RequireComponent(typeof(PlayerDisplay))]
public class PlayerStateMgr : MonoBehaviour {
    private StateMgr<PlayerState, PlayerStateMgr> m_StateMgr = null;
	private PlayerDisplay m_PlayerDisplay = null;
    private bool m_HasFirstChangedStated = false;
    private bool m_IsCnsState = false;
	private CNSStateDef m_CurrentCnsDef = null;
	private CNSStateDef m_PrevCnsDef = null;

    public void SetCnsState(bool isUseCns)
    {
        m_IsCnsState = isUseCns;
    }

	public bool IsCnsState
	{
		get {
			return m_IsCnsState;
		}
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

	private void ApplyPrevCnsDef()
	{
		m_PrevCnsDef = m_CurrentCnsDef;
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
        if (!isCns)
        {
			if (m_CurrentCnsDef == null && m_StateMgr.CurrStateKey == state && m_HasFirstChangedStated)
                return false;
        }
		CNSStateDef selDef = null;
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
			selDef = def;
		}

		ApplyPrevCnsDef ();
		m_CurrentCnsDef = selDef;

        m_IsCnsState = isCns;
		bool ret = m_StateMgr.ChangeState(state, m_IsCnsState);
		if (ret)
			m_CurrentCnsDef = selDef;
		else
			m_CurrentCnsDef = null;
		return ret;
    }

	public void ClearCurrentCnsDef()
	{
		m_PrevCnsDef = null;
		m_CurrentCnsDef = null;
	}

	public int PrevStateNo
	{
		get {
			if (m_PrevCnsDef == null)
				return (int)PlayerState.psNone;
			return m_PrevCnsDef.Animate;
		}
	}

	public int StateNo
	{
		get
		{
			if (m_CurrentCnsDef == null)
				return (int)PlayerState.psNone;
			return m_CurrentCnsDef.Animate;
		}
	}

	public CNSStateDef CurrentCnsDef
	{
		get {
			return m_CurrentCnsDef;
		}
		set
		{
			if (m_CurrentCnsDef != value) {
				ApplyPrevCnsDef ();
				m_CurrentCnsDef = value;
			}
		}
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
