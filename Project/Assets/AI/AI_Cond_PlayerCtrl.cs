using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/条件/角色Ctrl状态")]
	[Serializable]
	public class AI_Cond_PlayerCtrl: AI_BaseCondition
	{
		[SerializeField] public int Ctrl = 1;
		[SerializeField] public bool isNot = false;	

		public override string ToCondString(string luaPlayer)
		{
			string ret = string.Format ("trigger:Ctrl({0})", luaPlayer);
			if (isNot)
				ret += " ~= ";
			else
				ret += " == ";
			ret += Ctrl.ToString ();
			return ret;
		}
	}
}
