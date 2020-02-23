using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/条件/角色是否Alive")]
	[Serializable]
	public class AI_Cond_PlayerIsAlive: AI_BaseCondition
	{
		[SerializeField] public bool isNot = false;

		public override string ToCondString(string luaPlayer)
		{
			string ret = string.Format ("trigger:Alive({0})", luaPlayer);
			if (isNot)
				ret = "not " + ret;
			return ret;
		}
	}
}