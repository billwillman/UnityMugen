using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/条件/角色爆炸存在")]
	[Serializable]
	public class AI_Cond_Numexplod : AI_BaseCondition {
		[SerializeField] public int id = -1;
		[SerializeField] public bool isNot = false;

		public override string ToCondString(string luaPlayer)
		{
			if (id < 0)
				return string.Empty;
			string ret = string.Format ("trigger:Numexplod({0}, {1:D})", luaPlayer, id);
			if (isNot)
				ret = "not " + ret;
			return ret;
		}
	}
}