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
    left = 0x1,
    right = 0x2,
    jump = 0x4,
    down = 0x8,
	attack1 = 0x10,
	attack2 = 0x20,
	attack3 = 0x40,
	attack4 = 0x80,
	attack5 = 0x100,
	attack6 = 0x200,
}

public enum InputStateType
{
    none = 0
}

public struct InputValue
{
	public int keyCodeValue;
	public float tick;
}
		

public class InputControl: MonoBehaviour
{
    private IInputListener m_Listener;
    private Dictionary<int, int> m_KeyControlMap = new Dictionary<int, int>();
	private Dictionary<int, List<InputValue>> m_KeyMsgMap = new Dictionary<int, List<InputValue>>();
    private Dictionary<int, int> m_RuntimePlayerKeyValueMap = new Dictionary<int, int>();

    public bool m_ShowInput = false;

    void Awake()
    {
        RegisterDefaultKeyControls();
    }

	private int GetPlayerNoAndInputValue(byte player, int keyCode, bool checkPress, bool canCombine)
    {
		int press = checkPress ? 1 : 0;
		int combine = canCombine ? 1 : 0;
		int ret = (((int)player) << 24) | (keyCode & 0xFFFF) | ((press) << 16) | ((combine) << 17);
        return ret;
    }

	private int GetPlayerNoAndInputValue(InputPlayerType player, InputControlType keyCode, bool checkPress, bool canCombine)
    {
		return GetPlayerNoAndInputValue((byte)player, (int)keyCode, checkPress, canCombine);
    }

    private InputControlType GetControlType(int value)
    {
        InputControlType ret = (InputControlType)(value & 0xFFFF);
        return ret;
    }

	private bool GetKeyCanCombine(int value)
	{
		return ((value >> 17) & 0x1) != 0;
	}

    private bool GetKeyCanPress(int value)
    {
        return ((value >> 16) & 0x1) != 0;
    }

	private InputPlayerType GetPlayerType(int value)
	{
		return (InputPlayerType)((value >> 24) & 0xff);
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

		m_KeyControlMap[(int)KeyCode.U] = GetPlayerNoAndInputValue(InputPlayerType._1p, InputControlType.attack1, false, false);
		m_KeyControlMap[(int)KeyCode.I] = GetPlayerNoAndInputValue(InputPlayerType._1p, InputControlType.attack2, false, false);
		m_KeyControlMap[(int)KeyCode.O] = GetPlayerNoAndInputValue(InputPlayerType._1p, InputControlType.attack3, false, false);
		m_KeyControlMap[(int)KeyCode.J] = GetPlayerNoAndInputValue(InputPlayerType._1p, InputControlType.attack4, false, false);
		m_KeyControlMap[(int)KeyCode.K] = GetPlayerNoAndInputValue(InputPlayerType._1p, InputControlType.attack5, false, false);
		m_KeyControlMap[(int)KeyCode.L] = GetPlayerNoAndInputValue(InputPlayerType._1p, InputControlType.attack6, false, false);

        m_KeyControlMap[(int)KeyCode.UpArrow] = GetPlayerNoAndInputValue(InputPlayerType._2p, InputControlType.jump, true, true);
        m_KeyControlMap[(int)KeyCode.LeftArrow] = GetPlayerNoAndInputValue(InputPlayerType._2p, InputControlType.left, true, true);
        m_KeyControlMap[(int)KeyCode.RightArrow] = GetPlayerNoAndInputValue(InputPlayerType._2p, InputControlType.right, true, true);
        m_KeyControlMap[(int)KeyCode.DownArrow] = GetPlayerNoAndInputValue(InputPlayerType._2p, InputControlType.down, true, true);
    }
    
    public void AttachListener(IInputListener listener)
    {
        m_Listener = listener;
    }

	private void SendPlayerKeyControl(InputPlayerType type, int value)
	{
        if (type == InputPlayerType.none || value == 0)
			return;
		
		int key = (int)type;
		List<InputValue> list;
		if (!m_KeyMsgMap.TryGetValue (key, out list)) {
			list = new List<InputValue> ();
			m_KeyMsgMap [key] = list;
		}

		InputValue input = new InputValue();
		input.keyCodeValue = value;
		input.tick = Time.realtimeSinceStartup;
		list.Add (input);
	}

    private string GetInputControlTypeStr(int value)
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        if ((value & (int)InputControlType.left) != 0)
            builder.Append('←');
        else
        if ((value & (int)InputControlType.right) != 0)
            builder.Append('→');

        if ((value & (int)InputControlType.jump) != 0)
            builder.Append('↑');
        else
            if ((value & (int)InputControlType.down) != 0)
                builder.Append('↓');
        return builder.ToString();
    }

    void OnGUI()
    {
        if (!m_ShowInput)
            return;

        Rect inputLabelRect = new Rect(10f, 10f, 200f, 800f);
        GUILayout.BeginArea(inputLabelRect);
        GUILayout.BeginVertical();
        GUILayout.Label("【Player Control】");
        var iter = m_RuntimePlayerKeyValueMap.GetEnumerator();
        while (iter.MoveNext())
        {
            int ctlType = iter.Current.Value;
            string s = GetInputControlTypeStr(ctlType);
            s = string.Format("[{0:D}]:{1}", iter.Current.Key, s);
            GUILayout.Label(s);
        }
        iter.Dispose();

        GUILayout.Label("【Player Wugong】");

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

	private void CheckInputValueTime(InputPlayerType type, float removeTime = 0.5f)
	{
		if (type == InputPlayerType.none)
			return;

        PlayerDisplay display = GetPlayer(type);
        if (display == null)
            return;

		int key = (int)type;
		List<InputValue> list;
		if (!m_KeyMsgMap.TryGetValue (key, out list) || list.Count <= 0)
			return;
		while (list.Count > 0) {
			var input = list [0];
			if (UnityEngine.Time.unscaledTime - input.tick < 0.5f)
				break;
			list.RemoveAt (0);
		}

	}

    private int PreProcessInputValue(int value)
    {
        if (value == 0)
            return value;

        // 特殊处理同时按下不显示的
        int v1 = (value & (int)InputControlType.jump);
        int v2 = (value & (int)InputControlType.down);
        if ((v1 != 0) && (v2 != 0))
        {
            int updown = (int)InputControlType.jump | (int)InputControlType.down;
            value = value & (~updown);
        }

        v1 = (value & (int)InputControlType.left);
        v2 = (value & (int)InputControlType.right);
        if ((v1 != 0) && (v2 != 0))
        {
            int leftright = (int)InputControlType.left | (int)InputControlType.right;
            value = value & (~leftright);
        }
        return value;
    }

	void CheckInputs(InputPlayerType playerType)
    {
		var player = GetPlayer (playerType);
		if (player == null || !player.CanInputKey())
			return;

        int value = 0;
        // 判断Press
        var iter = m_KeyControlMap.GetEnumerator();
        while (iter.MoveNext())
        {
            if (GetPlayerType (iter.Current.Value) == playerType)
            {
                KeyCode key = (KeyCode)iter.Current.Key;
                if (Input.GetKey(key))
                {
                    if (GetKeyCanPress(iter.Current.Value))
                    {
                        int v = (int)GetControlType(iter.Current.Value);
                        value |= v;
                    }
                }
            }
        }
        iter.Dispose();
        
        // 按下优先级高于Press
		iter = m_KeyControlMap.GetEnumerator();
		
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

        value = PreProcessInputValue(value);

        // 发送给操作给角色，每个按键先发送给角色， 招式通过其他再判断
        m_RuntimePlayerKeyValueMap[(int)playerType] = value;
		if (value != 0)
			SendPlayerKeyControl (playerType, value);
    }

    private float m_CheckInputTime = 0f;

    void Update()
    {
        float time = Time.realtimeSinceStartup;
        if (time - m_CheckInputTime >= _cCheckInputDeltaTime)
        {
            m_CheckInputTime = time;

            CheckInputs(InputPlayerType._1p);
            CheckInputs(InputPlayerType._2p);
            CheckInputs(InputPlayerType._3p);
            CheckInputs(InputPlayerType._4p);
        }

        CheckInputValueTime(InputPlayerType._1p, _cInputRemoveTime);
        CheckInputValueTime(InputPlayerType._2p, _cInputRemoveTime);
        CheckInputValueTime(InputPlayerType._3p, _cInputRemoveTime);
        CheckInputValueTime(InputPlayerType._4p, _cInputRemoveTime);

        CheckWuGong(InputPlayerType._1p, _cWuGongCheckTime);
        CheckWuGong(InputPlayerType._2p, _cWuGongCheckTime);
        CheckWuGong(InputPlayerType._3p, _cWuGongCheckTime);
        CheckWuGong(InputPlayerType._4p, _cWuGongCheckTime);
    }

    void CheckWuGong(InputPlayerType player, float checkTime)
    {

    }

    private static readonly float _cCheckInputDeltaTime = 0.01f;
    private static readonly float _cInputRemoveTime = 0.3f;
    private static readonly float _cWuGongCheckTime = 0.1f;
}