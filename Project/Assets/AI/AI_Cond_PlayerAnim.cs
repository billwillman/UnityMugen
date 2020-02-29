using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/条件/角色动画Anim编号")]
	[Serializable]
	public class AI_Cond_PlayerAnim : AI_BaseCondition {
		[SerializeField] public int aniNo = CNSStateDef._cNoVaildAnim;
		[SerializeField] public AI_Cond_Op op = AI_Cond_Op.Equal;

		public override string ToCondString(string luaPlayer)
		{
			if (aniNo == CNSStateDef._cNoVaildAnim)
				return string.Empty;
			string ret = string.Format ("trigger:Anim({0}){1}{2:D}", luaPlayer, GetOpStr(op), aniNo);
			return ret;
		}
	}
}
