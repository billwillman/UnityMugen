using System;
using UnityEngine;

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
    }
}