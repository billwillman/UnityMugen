using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;

[RequireComponent(typeof(ImageAnimation))]
public class PlayerPartMgr : MonoBehaviour {

	public List<PlayerPart> m_PartList = null;
	
	public void OnUpdateFrame(ImageAnimation target)
	{
		if (m_PartList != null) {
			for (int i = 0; i < m_PartList.Count; ++i) {
				var part = m_PartList [i];
				if (part != null)
					part.OnUpdateFrame (target);
			}
		}
	}
}

