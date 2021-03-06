﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;

[RequireComponent(typeof(ImageAnimation))]
public class PlayerPartMgr : MonoBehaviour {

	public List<PlayerPart> m_PartList = null;

	public void AddPart(PlayerPart part)
	{
		if (part == null)
			return;
		if (m_PartList == null)
			m_PartList = new List<PlayerPart> ();
		m_PartList.Add (part);
	}

	public void RemovePart(PlayerPart part)
	{
		if (m_PartList == null)
			return;
		m_PartList.Remove (part);
	}

	public bool ContainsExplod(int explodId)
	{
		if (explodId < 0 || m_PartList == null)
			return false;

		for (int i = m_PartList.Count - 1; i >= 0; --i) {
			var part = m_PartList [i];
			if (part != null) {
				Explod explod = part as Explod;
				if (explod != null && explod.ID == explodId) {
					return true;
				}
			}
		}
		return false;
	}

	public void RemoveExplod(int explodId)
	{
		if (explodId < 0 || m_PartList == null)
			return;
		for (int i = m_PartList.Count - 1; i >= 0; --i) {
			var part = m_PartList [i];
			if (part != null) {
				Explod explod = part as Explod;
				if (explod != null && explod.ID == explodId) {
					part.InteralDoDestroy ();
				}
			}
		}
	}
	
	public void OnUpdateFrame(ImageAnimation target)
	{
		if (m_PartList != null) {
			for (int i = 0; i < m_PartList.Count; ++i) {
				var part = m_PartList [i];
				if (part != null)
					part.OnParentUpdateFrame (target);
			}
		}
	}

	public void ResetChangeStatePart()
	{
		if (m_PartList != null) {
			for (int i = m_PartList.Count - 1; i >= 0; --i) {
				var part = m_PartList [i];
				if (part != null && part.isChangeStateRemove)
					part.InteralDoDestroy ();
			}
		}
	}

	public void OnFrameEnd(ImageAnimation target)
	{
		if (m_PartList != null) {
			for (int i = 0; i < m_PartList.Count; ++i) {
				var part = m_PartList [i];
				if (part != null)
					part.OnParentFrameEnd (target);
			}
		}
	}

	public void OnFramePosUpdate(ImageAnimation target)
	{
		if (m_PartList != null) {
			for (int i = 0; i < m_PartList.Count; ++i) {
				var part = m_PartList [i];
				if (part != null)
					part.OnParentFramePosUpdate (target);
			}
		}
	}
}

