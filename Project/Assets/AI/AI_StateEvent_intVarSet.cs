using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/设置int var")]
	[Serializable]
	public class AI_StateEvent_intVarSet: AI_CreateStateEvent
	{
		[SerializeField] public int index;
		[SerializeField] public int value;

		protected override string GetDoStr(bool hasCond)
		{
			string ret = string.Format ("trigger:VarSet(luaPlayer, {0:D}, {1:D})", index, value);
			return ret;
		}
	}
}
