using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/条件/float var")]
	[Serializable]
	public class AI_Cond_Player_FloatVar: AI_BaseCondition
	{
		[SerializeField] public int index;
		[SerializeField] public float value;
		[SerializeField] public AI_Cond_Op op = AI_Cond_Op.Equal;

		public override string ToCondString(string luaPlayer)
		{
			var opStr = GetOpStr (op);
			string ret = string.Format ("trigger:fVar({0}, {1:D}){2}{3}", luaPlayer, index, opStr, value.ToString());
			return ret;
		}
	}
}
