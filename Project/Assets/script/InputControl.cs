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
	private Dictionary<int, int> m_KeyControlMap = new Dictionary<int, int>();
   
    void Awake()
    {
        RegisterDefaultKeyControls();
    }

	private int GetPlayerNoAndInputValue(byte player, int keyCode, bool checkPress, bool canCombine)
    {
		int press = checkPress ? 1 : 0;
		int combine = canCombine ? 1 : 0;
		int ret = (((int)player) << 24) | (keyCode & 0x000000ff) | ((press) << 16) | ((combine) << 8);
        return ret;
    }

	private int GetPlayerNoAndInputValue(InputPlayerType player, InputControlType keyCode, bool checkPress, bool canCombine)
    {
		return GetPlayerNoAndInputValue((byte)player, (int)keyCode, checkPress, canCombine);
    }

	private bool GetKeyCanCombine(int value)
	{
		return ((value >> 8) & 0xff) != 0;
	}

	private InputPlayerType GetPlayerType(int value)
	{
		return (InputPlayerType)((value >> 24) & 0xff);
	}

	private InputControlType GetControlType(int value)
	{
		return (InputControlType)((value >> 16) & 0xff);
	}

	private PlayerDisplay GetPlayer(int value)
	{
		InputPlayerType type = GetPlayerType (value);
		return GetPlayer (type);
	}

	private PlayerDisplay GetPlayer(InputPlayerType type)
	{
		switch (type) {
		case InputPlayerType._1p:
			return PlayerControls.GetInstance ().m_1P;
		case InputPlayerType._2p:
			return PlayerControls.GetInstance ().m_2P;
		case InputPlayerType._3p:
			return PlayerControls.GetInstance ().m_3P;
		case InputPlayerType._4p:
			return PlayerControls.GetInstance ().m_4P;
		}
		return null;
	}

    private void RegisterDefaultKeyControls()
    {
		m_KeyControlMap[(int)KeyCode.W] = GetPlayerNoAndInputValue(InputPlayerType._1p, InputControlType.jump, true, true);
		m_KeyControlMap[(int)KeyCode.A] = GetPlayerNoAndInputValue(InputPlayerType._1p, InputControlType.left, true, true);
		m_KeyControlMap[(int)KeyCode.D] = GetPlayerNoAndInputValue(InputPlayerType._1p, InputControlType.right, true, true);
		m_KeyControlMap[(int)KeyCode.S] = GetPlayerNoAndInputValue(InputPlayerType._1p, InputControlType.down, true, true);
    }
    
    public void AttachListener(IInputListener listener)
    {
        m_Listener = listener;
    }

	private void SendPlayerKeyControl(InputPlayerType type, int value)
	{
	}

	void CheckInputs(InputPlayerType playerType)
    {
		var player = GetPlayer (playerType);
		if (player == null || !player.CanInputKey())
			return;
		var iter = m_KeyControlMap.GetEnumerator();
		int value = 0;
        while (iter.MoveNext())
        {
			if (GetPlayerType (iter.Current.Value) == playerType) {
				KeyCode key = (KeyCode)iter.Current.Key;
				if (Input.GetKeyDown (key)) {
					int v = (int)GetControlType (iter.Current.Value);
					if (GetKeyCanCombine (iter.Current.Value))
						value |= v;
					else {
						value = v;
						break;
					}
				}
			}
        }
        iter.Dispose();

		if (value != 0)
			SendPlayerKeyControl (playerType, value);
    }

    void Update()
    {
		CheckInputs (InputPlayerType._1p);
		CheckInputs (InputPlayerType._2p);
		CheckInputs (InputPlayerType._3p);
		CheckInputs (InputPlayerType._4p);
    }
}