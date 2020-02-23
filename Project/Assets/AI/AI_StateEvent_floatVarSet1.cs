using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/设置float var")]
	[Serializable]
	public class AI_StateEvent_floatVarSet: AI_CreateStateEvent
	{
		[SerializeField] public int index;
		[SerializeField] public float value;

		protected override string GetDoStr(bool hasCond)
		{
			string ret = string.Format ("trigger:fVarSet(luaPlayer, {0:D}, {1})", index, value.ToString());
			return ret;
		}
	}
}
