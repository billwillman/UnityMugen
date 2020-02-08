using UnityEngine;
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

	public void ResetPart()
	{
		if (m_PartList != null) {
			for (int i = m_PartList.Count - 1; i >= 0; --i) {
				var part = m_PartList [i];
				if (part != null)
					GameObject.Destroy (part.gameObject);
			}
			m_PartList.Clear ();
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
}

