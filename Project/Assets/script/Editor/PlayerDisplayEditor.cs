using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Mugen;

public struct SelctedItem
{
    public SelctedItem(int stateIdx = -1, int palIdx = -1)
    {
        stateIndex = stateIdx;
        palletIndex = palIdx;
    }
    public int stateIndex ;
    public int palletIndex;
}

[CustomEditor(typeof(PlayerDisplay))]
public class PlayerDisplayEditor : Editor {
    private List<PlayerState> m_VaildStateList = null;
    private string[] m_VaildPalletNameList = null;
    private string[] m_VaildStateNameList = null;
    private PlayerDisplay m_LastDisplay = null;
    private int m_StateSelected = -1;
    private int m_PalletSelectd = -1;
    private static Dictionary<int, SelctedItem> m_SelectedMap = new Dictionary<int, SelctedItem>();

    private void InitPlayerDisplay()
    {
        m_VaildStateList = null;
        m_VaildStateNameList = null;
        m_VaildPalletNameList = null;
        m_StateSelected = -1;
        m_PalletSelectd = -1;
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
            var iter = player.AirCfg.GetStateIter();
            while (iter.MoveNext())
            {
                if (m_VaildStateList == null)
                    m_VaildStateList = new List<PlayerState>();
                m_VaildStateList.Add(iter.Current.Key);
            }
            iter.Dispose();
            if (m_VaildStateList  != null && m_VaildStateList.Count > 0)
            {
                m_VaildStateNameList = new string[m_VaildStateList.Count];
                for (int i = 0; i < m_VaildStateList.Count; ++i)
                {
                    var state = m_VaildStateList[i];
                    m_VaildStateNameList[i] = state.ToString();
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
                if (m_LastDisplay.PlayAni(state, true))
                {
                    m_StateSelected = newSelected;
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

		int curFrame = m_LastDisplay.ImageCurrentFrame;
		EditorGUILayout.LabelField ("动画当前帧", curFrame.ToString ());
        string palletName = m_LastDisplay.PalletName;
        EditorGUILayout.LabelField("当前调色板", palletName);
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
