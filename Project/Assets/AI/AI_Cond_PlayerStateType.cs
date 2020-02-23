using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/条件/角色StateType状态")]
	[Serializable]
	public class AI_Cond_PlayerStateType: AI_BaseCondition
	{
		[SerializeField] public Cns_Type stateType = Cns_Type.S;

		[SerializeField] public bool isNot = false;	

		public override string ToCondString(string luaPlayer)
		{
			string ret = string.Format ("{0}.{1}", stateType.GetType ().FullName, stateType.ToString ());
			if (isNot)
				ret = "trigger:Statetype ~= " + ret;
			else
				ret = "trigger:Statetype == " + ret;
			return ret;
		}
	}
}
