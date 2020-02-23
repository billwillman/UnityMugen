using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{

[CreateNodeMenu("AI/条件/角色AnimElem")]
[Serializable]
public class AI_Cond_PlayerAniElem: AI_BaseCondition
{
	[SerializeField] public int aniElem;
	[SerializeField] public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:AnimElem({0}){1}{2:D}", luaPlayer, opStr, aniElem);
		return ret;
	}
}

}
