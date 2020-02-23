using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/PhysicsType设置")]
	[Serializable]
	public class AI_StateEvent_PhysicsTypeSet: AI_CreateStateEvent
	{
		[SerializeField] public Cns_PhysicsType physicsType = Cns_PhysicsType.none;

		protected override string GetDoStr(bool hasCond)
		{
			if (physicsType == Cns_PhysicsType.none)
				return string.Empty;

			string ret = string.Format ("trigger:PhysicsTypeSet(luaPlayer, {0}.{1})", physicsType.GetType().FullName, physicsType.ToString());
			return ret;
		}
	}
}