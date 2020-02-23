using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/条件/角色动画Ani是否存在")]
	[Serializable]
	public class AI_Cond_PlayerAniExist: AI_BaseCondition
	{
		[SerializeField] public int value;
		[SerializeField] public bool isNot = false;

		public override string ToCondString(string luaPlayer)
		{
			string ret = string.Format ("trigger:AnimExist({0}, {1:D})", luaPlayer, value);
			if (isNot)
				ret = "not " + ret;
			return ret;
		}
	}
}