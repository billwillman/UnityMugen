using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInputListener
{
    void OnKeyDown();
    void OnKeyUp();
    void OnKeyPress();
}

public enum InputPlayerType
{
    none = 0,
    _1p = 1,
    _2p,
    _3p,
    _4p
}

public enum InputControlType
{
    none = 0,
    up = 0x2,
    left = 0x4,
    right = 0x8,
    jump = 0x10,
    down = 0x20,
}

public enum InputStateType
{
    none = 0
}

public class InputControl: MonoBehaviour
{
    private IInputListener m_Listener;
    private Dictionary<int, int> m_KeyControl = new Dictionary<int, int>();
   
    void Awake()
    {
        RegisterDefaultKeyControls();
    }

    private int GetPlayerNoAndInputValue(byte player, int keyCode)
    {
        int ret = (((int)player) << 24) | (keyCode & 0x00ffffff);
        return ret;
    }

    private int GetPlayerNoAndInputValue(InputPlayerType player, InputControlType keyCode)
    {
        return GetPlayerNoAndInputValue((byte)player, (int)keyCode);
    }

    private void RegisterDefaultKeyControls()
    {
        m_KeyControl[(int)KeyCode.W] = GetPlayerNoAndInputValue(InputPlayerType._1p, InputControlType.jump);
        m_KeyControl[(int)KeyCode.A] = GetPlayerNoAndInputValue(InputPlayerType._1p, InputControlType.left);
        m_KeyControl[(int)KeyCode.D] = GetPlayerNoAndInputValue(InputPlayerType._1p, InputControlType.right);
        m_KeyControl[(int)KeyCode.S] = GetPlayerNoAndInputValue(InputPlayerType._1p, InputControlType.down);
    }
    
    public void AttachListener(IInputListener listener)
    {
        m_Listener = listener;
    }

    private void SendKeyMessage(KeyCode key, int type)
    {

    }

    void CheckInputs()
    {
        var iter = m_KeyControl.GetEnumerator();
        while (iter.MoveNext())
        {
            KeyCode key = (KeyCode)iter.Current.Key;
            
        }
        iter.Dispose();
    }

    void Update()
    {
        CheckInputs();
    }
}