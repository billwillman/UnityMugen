using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/条件/角色PrevStateNo")]
	[Serializable]
	public class AI_Cond_Player_PlayerPrevStateNo: AI_BaseCondition
	{
		[SerializeField] public int prevStateNo;
		[SerializeField] public AI_Cond_Op op = AI_Cond_Op.Equal;

		public override string ToCondString(string luaPlayer)
		{
			var opStr = GetOpStr (op);
			string ret = string.Format ("trigger:PrevStateNo({0}){1}{2:D}", luaPlayer, opStr, prevStateNo);
			return ret;
		}
	}
}
