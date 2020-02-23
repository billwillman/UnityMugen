using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/条件/角色AnimElemTime")]
	[Serializable]
	public class AI_Cond_PlayerAnimElemTime: AI_BaseCondition
	{
		[SerializeField] public int frameNo;
		[SerializeField] public int animElem;
		[SerializeField] public AI_Cond_Op op = AI_Cond_Op.Equal;

		public override string ToCondString(string luaPlayer)
		{
			var opStr = GetOpStr (op);
			string ret = string.Format ("trigger:AnimElemTime({0}, {1:D}){2}{3:D}", luaPlayer, frameNo, opStr, animElem);
			return ret;
		}
	}
}
