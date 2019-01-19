using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Mugen;

public struct SelctedItem
{
	public SelctedItem(int stateIdx = -1, int palIdx = -1, bool clsn = false)
    {
        stateIndex = stateIdx;
        palletIndex = palIdx;
		showClsn = clsn;
    }
    public int stateIndex ;
    public int palletIndex;
	public bool showClsn;
}

[CustomEditor(typeof(PlayerDisplay))]
public class PlayerDisplayEditor : Editor {
    private List<PlayerState> m_VaildStateList = null;
    private string[] m_VaildPalletNameList = null;
    private string[] m_VaildStateNameList = null;
    private PlayerDisplay m_LastDisplay = null;
    private int m_StateSelected = -1;
    private int m_PalletSelectd = -1;
	private bool m_ShowClsn = false;
    private static Dictionary<int, SelctedItem> m_SelectedMap = new Dictionary<int, SelctedItem>();

    private void InitPlayerDisplay()
    {
        m_VaildStateList = null;
        m_VaildStateNameList = null;
        m_VaildPalletNameList = null;
        m_StateSelected = -1;
        m_PalletSelectd = -1;
		m_ShowClsn = false;
        if (m_LastDisplay == null)
            return;
        var player = m_LastDisplay.GPlayer;
        if (player == null)
        {
            // 可能还没有加载完数据
            m_LastDisplay = null;
            return;
        }
        if (player.AirCfg != null && player.AirCfg.IsVaild)
        {
            for (int i = 0; i < player.AirCfg.GetStateCount(); ++i)
            {
                var key = player.AirCfg.GetStateByIndex(i);
                if (m_VaildStateList == null)
                    m_VaildStateList = new List<PlayerState>();
                m_VaildStateList.Add(key);
            }

            // 刪除無效狀態
            for (int i = m_VaildStateList.Count - 1; i >= 0; --i)
            {
                var state = m_VaildStateList[i];
                if (!m_LastDisplay.HasStateImage(state, true))
                    m_VaildStateList.RemoveAt(i);
            }
            //--------------------------

            if (m_VaildStateList != null && m_VaildStateList.Count > 0)
            {
                m_VaildStateNameList = new string[m_VaildStateList.Count];
                for (int i = 0; i < m_VaildStateList.Count; ++i)
                {
                    var state = m_VaildStateList[i];
                    m_VaildStateNameList[i] = string.Format("{0}({1:D})", state.ToString(), (int)state);
                }
            }

            SelctedItem selItem;
            if (m_SelectedMap.TryGetValue(m_LastDisplay.GetInstanceID(), out selItem))
            {
                var state = m_VaildStateList[selItem.stateIndex];
                if (m_LastDisplay.PlayAni(state, true))
                    m_StateSelected = selItem.stateIndex;
            }
        }

        var loaderPlayer = m_LastDisplay.LoaderPlayer;
        if (loaderPlayer != null && loaderPlayer.PalNameList != null)
        {
            m_VaildPalletNameList = loaderPlayer.PalNameList.ToArray();
            SelctedItem selItem;
            if (m_SelectedMap.TryGetValue(m_LastDisplay.GetInstanceID(), out selItem))
            {
                m_PalletSelectd = selItem.palletIndex;
            }
        }

		SelctedItem selItem1;
		if (m_SelectedMap.TryGetValue (m_LastDisplay.GetInstanceID (), out selItem1)) {
			m_ShowClsn = selItem1.showClsn;
		}
    }

    private void DrawPlayerDisplay()
    {
        if (m_LastDisplay == null)
            return;
        if (m_VaildStateList != null && m_VaildStateNameList != null)
        {
            int newSelected = EditorGUILayout.Popup("手动选择角色动作", m_StateSelected, m_VaildStateNameList);
            if (m_StateSelected != newSelected)
            {
                var state = m_VaildStateList[newSelected];
                m_StateSelected = newSelected;
                if (m_LastDisplay.PlayAni(state, true))
                {
                    SelctedItem item;
                    if (!m_SelectedMap.TryGetValue(m_LastDisplay.GetInstanceID(), out item))
                        item = new SelctedItem();
                    item.stateIndex = m_StateSelected;
                    m_SelectedMap[m_LastDisplay.GetInstanceID()] = item;
                }
            }
        }

        if (m_VaildPalletNameList != null)
        {
            int newSelected = EditorGUILayout.Popup("手动选择调色板", m_PalletSelectd, m_VaildPalletNameList);
            if (m_PalletSelectd != newSelected)
            {
                string newPalletName = m_VaildPalletNameList[newSelected];
                m_PalletSelectd = newSelected;
                
                SelctedItem item;
                if (!m_SelectedMap.TryGetValue(m_LastDisplay.GetInstanceID(), out item))
                    item = new SelctedItem();
                item.palletIndex = m_PalletSelectd;
                m_SelectedMap[m_LastDisplay.GetInstanceID()] = item;

                m_LastDisplay.PalletName = newPalletName;
            }
        }

		bool newShowClsn = EditorGUILayout.Toggle ("显示包围盒", m_ShowClsn);
		if (m_ShowClsn != newShowClsn) {
			m_ShowClsn = newShowClsn;
			SelctedItem item;
			if (!m_SelectedMap.TryGetValue(m_LastDisplay.GetInstanceID(), out item))
				item = new SelctedItem();
			item.showClsn = newShowClsn;
			m_SelectedMap[m_LastDisplay.GetInstanceID()] = item;
			ShowClsn (newShowClsn);
		}

		int curFrame = m_LastDisplay.ImageCurrentFrame;
		EditorGUILayout.LabelField ("动画当前帧", curFrame.ToString ());
        string palletName = m_LastDisplay.PalletName;
        string str = palletName;
        if (string.IsNullOrEmpty(str))
            str = "(无)";
        EditorGUILayout.LabelField("当前调色板", str);
        if (!string.IsNullOrEmpty(palletName) && m_PalletSelectd < 0)
        {
            for (int i = 0; i < m_VaildPalletNameList.Length; ++i)
            {
                if (string.Compare(palletName, m_VaildPalletNameList[i], true) == 0)
                {
                    m_PalletSelectd = i;
                    break;
                }
            }
        }
    }

	void ShowClsn(bool isShow)
	{
		var target = this.target as PlayerDisplay;
		if (target == null)
			return;
		
	}

	public override void OnInspectorGUI ()
    {
        DrawDefaultInspector();

        if (Application.isPlaying)
        {
            if (m_LastDisplay != this.target)
            {
                m_LastDisplay = this.target as PlayerDisplay;
                InitPlayerDisplay();
            }

            DrawPlayerDisplay();

        } else
        {
            if (m_SelectedMap.Count > 0)
                m_SelectedMap.Clear();
        }

    }
}
