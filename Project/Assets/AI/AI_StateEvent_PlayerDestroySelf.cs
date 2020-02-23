using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/删除角色自己")]
	[Serializable]
	public class AI_StateEvent_PlayerDestroySelf: AI_CreateStateEvent
	{
		protected override string GetDoStr(bool hasCond)
		{
			return "trigger:DestroySelf(luaPlayer)";
		}
	}
}
