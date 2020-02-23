using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/条件/角色Power值")]
	[Serializable]
	public class AI_Cond_PlayerPower: AI_BaseCondition
	{
		[SerializeField] public int value;
		[SerializeField] public AI_Cond_Op op = AI_Cond_Op.Equal;

		public override string ToCondString(string luaPlayer)
		{
			var opStr = GetOpStr (op);
			string ret = string.Format ("trigger:Power({0}){1}{2:D}", luaPlayer, opStr, value);
			return ret;
		}
	}
}
