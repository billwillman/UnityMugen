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
	attack1 = 0x10, // x
	attack2 = 0x20, // y
	attack3 = 0x40, // d
	attack4 = 0x80, // s
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
	public float downTick;
}
		

public class InputControl: MonoBehaviour
{
    private IInputListener m_Listener;
    private Dictionary<int, int> m_KeyControlMap = new Dictionary<int, int>();
	private Dictionary<int, List<InputValue>> m_KeyMsgMap = new Dictionary<int, List<InputValue>>();
    private Dictionary<int, int> m_RuntimePlayerKeyValueMap = new Dictionary<int, int>();
	private int m_CtlMask = (int)InputControlType.left | (int)InputControlType.right | (int)InputControlType.jump | (int)InputControlType.down;

    public bool m_ShowInput = false;

	public void ClearKeys(InputPlayerType type)
	{
		if (type == InputPlayerType.none)
			return;
		
		List<InputValue> list;
		if (m_KeyMsgMap.TryGetValue ((int)type, out list))
			list.Clear ();
	}

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

	public List<InputValue> GetInputList(InputPlayerType type)
	{
		int key = (int)type;
		List<InputValue> list;
		if (!m_KeyMsgMap.TryGetValue (key, out list)) {
			list = null;
		}
		return list;
	}

	private void SendPlayerKeyControl(InputPlayerType type, int value, bool hasDown, float currentTime)
	{
        if (type == InputPlayerType.none || value == 0)
			return;
		
		int key = (int)type;
		List<InputValue> list;
		if (!m_KeyMsgMap.TryGetValue (key, out list)) {
			list = new List<InputValue> ();
			m_KeyMsgMap [key] = list;
		}

		if (!hasDown) {
			if (list.Count > 0) {
				var item = list [list.Count - 1];
				if (item.keyCodeValue == value && !hasDown) {
					item.tick = Time.realtimeSinceStartup;
					return;
				}

				int v = (item.keyCodeValue & value);
				if (v != 0) {
					value = value & (~v);
				}
				if (value == 0)
					return;
			}
		}
		InputValue input = new InputValue();
		input.keyCodeValue = value;
        input.tick = currentTime;
        if (hasDown)
        {
            input.downTick = input.tick;
           // Debug.LogError(input.tick.ToString());
        }
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

		if ((value & (int)InputControlType.attack1) != 0)
			builder.Append("X");

		if ((value & (int)InputControlType.attack2) != 0)
			builder.Append("Y");

		if ((value & (int)InputControlType.attack3) != 0)
			builder.Append("D");

		if ((value & (int)InputControlType.attack4) != 0)
			builder.Append("S");

        return builder.ToString();
    }

    private string GetPlayerWuGongStr(List<InputValue> values)
    {
        if (values == null || values.Count <= 0)
            return string.Empty;
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        for (int i = 0; i < values.Count; ++i)
        {
            var v = values[i];
            var value = v.keyCodeValue;
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
			
			if ((value & (int)InputControlType.attack1) != 0)
				builder.Append("X");

			if ((value & (int)InputControlType.attack2) != 0)
				builder.Append("Y");

			if ((value & (int)InputControlType.attack3) != 0)
				builder.Append("D");

			if ((value & (int)InputControlType.attack4) != 0)
				builder.Append("S");
        }

        return builder.ToString();
    }

    // 是否有允许按下的按键按下
    public bool IsVaildCanPressKeyPress(InputPlayerType playerType)
    {
        if (playerType == InputPlayerType.none)
            return false;
        var iter = m_KeyControlMap.GetEnumerator();
        while (iter.MoveNext())
        {
            if (GetPlayerType(iter.Current.Value) == playerType)
            {
                if (GetKeyCanPress(iter.Current.Value))
                {
                    KeyCode key = (KeyCode)iter.Current.Key;
                    bool ret = Input.GetKey(key);
                    if (ret)
                        return ret;
                }
            }
        }
        iter.Dispose();
        return false;
    }

	public int GetPlayerRunKeyValue(InputPlayerType type)
	{
		int key = (int)type;
		int ret;
		if (!m_RuntimePlayerKeyValueMap.TryGetValue (key, out ret))
			ret = 0;
		return ret;
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

        if (m_KeyMsgMap != null)
        {
            var it = m_KeyMsgMap.GetEnumerator();
            while (it.MoveNext())
            {
                int playerType = it.Current.Key;
                string s = GetPlayerWuGongStr(it.Current.Value);
                s = string.Format("[{0:D}]:{1}", playerType, s);
                GUILayout.Label(s);
            }
            it.Dispose();
        }

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

	private int PreProcessInputValue(int value, bool isHasDown)
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

		if (isHasDown) {
			
		}

        return value;
    }

	public bool GetKeyPress(InputPlayerType playerType, int ctlype)
	{
		return GetKeyPress (playerType, (InputControlType)ctlype);
	}

	public bool GetKeyPress(InputPlayerType playerType, InputControlType ctlype)
	{
		if (playerType == InputPlayerType.none || ctlype == InputControlType.none)
			return false;
		var iter = m_KeyControlMap.GetEnumerator ();
		while (iter.MoveNext ()) {
			if (GetPlayerType(iter.Current.Value) == playerType)
			{
				if (GetControlType(iter.Current.Value) == ctlype)
				{
					KeyCode key = (KeyCode)iter.Current.Key;
					return Input.GetKey (key);
				}
			}
		}
		iter.Dispose ();
		return false;
	}


    void CheckInputs(InputPlayerType playerType, bool checkPress, float currentTime)
    {
        var player = GetPlayer(playerType);
		if (player == null || !player.CanInputKey ()) {
			if (m_RuntimePlayerKeyValueMap.ContainsKey((int)playerType))
				m_RuntimePlayerKeyValueMap[(int)playerType] = 0;
			return;
		}



        int value = 0;
        int v1 = 0;

        bool canCheckPress = true;
        var keyList = GetInputList(playerType);
        float downAndPressDelta = 0;
        if (keyList != null && keyList.Count > 0)
        {
            InputValue lastKey = keyList[keyList.Count - 1];
            if (lastKey.downTick > 0)
            {
                downAndPressDelta = (currentTime - lastKey.downTick);
                canCheckPress = downAndPressDelta >= _cPressAndDownDeltaTime;
            }
        }

        if (canCheckPress)
        {
           // if (Mathf.Abs(downAndPressDelta) > float.Epsilon)
           //     Debug.LogError(downAndPressDelta.ToString());
            // 判断Press
            var iter = m_KeyControlMap.GetEnumerator();
            while (iter.MoveNext())
            {
                if (GetPlayerType(iter.Current.Value) == playerType)
                {
                    KeyCode key = (KeyCode)iter.Current.Key;
                    if (Input.GetKey(key) && !Input.GetKeyUp(key))
                    {
                        if (GetKeyCanPress(iter.Current.Value))
                        {
                            int v = (int)GetControlType(iter.Current.Value);
                            v1 |= v;
                            value |= v;
                        }
                    }
                }
            }
            iter.Dispose();
        }

        // 按下优先级高于Press
        var it = m_KeyControlMap.GetEnumerator();
		int v2 = 0;
		bool isNoCombine = false;
        while (it.MoveNext())
        {
			var plyType = GetPlayerType (it.Current.Value);
			if (plyType == playerType)
            {
                KeyCode key = (KeyCode)it.Current.Key;
				if (Input.GetKeyDown(key) && !Input.GetKeyUp(key))
                {
                    int v = (int)GetControlType(it.Current.Value);
					/*
					if (GetKeyCanCombine (it.Current.Value)) {
						value |= v;
						v2 |= v;
					}
                    else
                    {
						v = v & (~m_CtlMask);
						value = v;
						v2 = v;
                        v1 = 0;
                    }*/

                    // 检查一下间隔（如果上一个也是DOWN）
					
					value |= v;
					v2 |= v;
					if (!GetKeyCanCombine (it.Current.Value)) {
						isNoCombine = true;
					}
                }
            }
        }
        it.Dispose();

		if (isNoCombine) {
			value = value & (~m_CtlMask);
			v1 = 0;
			v2 = v2 & (~m_CtlMask);
		}

		int v3 = 0;
		it = m_KeyControlMap.GetEnumerator();
		while (it.MoveNext())
		{
			if (GetPlayerType(it.Current.Value) == playerType)
			{
				KeyCode key = (KeyCode)it.Current.Key;
				if (Input.GetKeyUp(key))
				{
					int v = (int)GetControlType(it.Current.Value);
					v3 |= v;
				}
			}
		}
		it.Dispose();

		if (v3 != 0) {
			v3 = ~v3;
			value = value & v3;
			v1 = v1 & v3;
			v2 = v2 & v3;
		}

		bool hasDown = (value & v2) != 0;
		value = PreProcessInputValue(value, hasDown);

        // 发送给操作给角色，每个按键先发送给角色， 招式通过其他再判断
        m_RuntimePlayerKeyValueMap[(int)playerType] = value;
        if (value != 0)
        {
            if (checkPress)
				SendPlayerKeyControl(playerType, value, hasDown, currentTime);
            else
            {
               // int v = value & (~v1);
				int v = v2;
                if (v != 0)
                    SendPlayerKeyControl(playerType, v, hasDown, currentTime);
            }
        }
    }

    private float m_CheckInputPressTime = 0f;
	private float m_CheckInputTime = 0f;

    void Update()
    {
        float time = Time.realtimeSinceStartup;

		float delta = time - m_CheckInputTime;
		bool isCheckInput = delta >= _cCheckInputDeltaTime;
		if (isCheckInput) {
			m_CheckInputTime = time;
			delta = time - m_CheckInputPressTime;
			bool isCheckPress = delta >= _cCheckInputPressDeltaTime;
			if (isCheckPress)
                m_CheckInputPressTime = time;
            CheckInputs(InputPlayerType._1p, isCheckPress, time);
            CheckInputs(InputPlayerType._2p, isCheckPress, time);
            CheckInputs(InputPlayerType._3p, isCheckPress, time);
            CheckInputs(InputPlayerType._4p, isCheckPress, time);
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

    public float _cCheckInputPressDeltaTime = 0.05f;
    public float _cCheckInputDeltaTime = 0.00f;
    public float _cInputRemoveTime = 0.5f;
    public float _cWuGongCheckTime = 0.1f;
    public float _cPressAndDownDeltaTime = 0.1f;
}