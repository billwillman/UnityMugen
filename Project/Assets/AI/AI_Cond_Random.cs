using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/条件/Random")]
	[Serializable]
	public class AI_Cond_Random: AI_BaseCondition
	{
		[SerializeField] public int randomValue = 1000;
		[SerializeField] public int value = 500;
		[SerializeField] public AI_Cond_Op op = AI_Cond_Op.Less;

		public override string ToCondString(string luaPlayer)
		{
			var opStr = GetOpStr (op);
			string ret = string.Format ("trigger:Random({0:D}){1}{2:D}", randomValue, opStr, value);
			return ret;
		}
	}
}
