using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/StateType设置")]
	[Serializable]
	public class AI_StateEvent_StateTypeSet: AI_CreateStateEvent
	{
		[SerializeField] public Cns_Type stateType = Cns_Type.S;

		protected override string GetDoStr(bool hasCond)
		{
			string ret = string.Format ("trigger:StateTypeSet(luaPlayer, {0}.{1})", stateType.GetType().FullName, stateType.ToString());
			return ret;
		}
	}
}
