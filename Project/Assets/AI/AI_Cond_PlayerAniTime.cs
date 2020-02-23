using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/条件/角色AnimTime")]
	[Serializable]
	public class AI_Cond_PlayerAniTime: AI_BaseCondition
	{
		[SerializeField] public int aniTime;
		[SerializeField] public AI_Cond_Op op = AI_Cond_Op.Equal;

		public override string ToCondString(string luaPlayer)
		{
			var opStr = GetOpStr (op);
			string ret = string.Format ("trigger:AnimTime({0}){1}{2:D}", luaPlayer, opStr, aniTime);
			return ret;
		}
	}
}

