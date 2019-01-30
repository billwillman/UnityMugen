using System;
using UnityEngine;
using Mugen;

[RequireComponent(typeof(InputControl))]
public class PlayerControls: MonoSingleton<PlayerControls>
{
    public PlayerDisplay m_1P = null;
    public PlayerDisplay m_2P = null;
    public PlayerDisplay m_3P = null;
    public PlayerDisplay m_4P = null;

    private InputControl m_InputCtl = null;

    public InputControl InputCtl
    {
        get
        {
            if (m_InputCtl == null)
                m_InputCtl = GetComponent<InputControl>();
            return m_InputCtl;
        }
    }

	public PlayerDisplay GetPlayer(int playerType)
	{
		return GetPlayer ((InputPlayerType)playerType);
	}

	public PlayerDisplay GetPlayer(InputPlayerType playerType)
	{
		PlayerDisplay ret = null;
		switch (playerType)
		{
		case InputPlayerType._1p:
			ret = m_1P;
			break;
		case InputPlayerType._2p:
			ret = m_2P;
			break;
		case InputPlayerType._3p:
			ret = m_3P;
			break;
		case InputPlayerType._4p:
			ret = m_4P;
			break;
		}
		return ret;
	}

    public void SwitchPlayer(InputPlayerType playerType, PlayerDisplay display)
    {
        switch (playerType)
        {
            case InputPlayerType._1p:
                m_1P = display;
                break;
            case InputPlayerType._2p:
                m_2P = display;
                break;
            case InputPlayerType._3p:
                m_3P = display;
                break;
            case InputPlayerType._4p:
                m_4P = display;
                break;
        }

        if (display != null && display.AnimationState == PlayerState.psNone)
        {
            display.ChangeState(PlayerState.psStand1);
            display.SetAutoCnsState();
        }
    }
}