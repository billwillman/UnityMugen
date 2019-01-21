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
}