using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/Ctrl设置")]
	[Serializable]
	public class AI_StateEvent_CtrlSet: AI_CreateStateEvent
	{
		[SerializeField] public int ctrl = 1;

		protected override string GetDoStr(bool hasCond)
		{
			string ret = string.Format ("trigger:CtrlSet(luaPlayer, {0:D})", ctrl);
			return ret;
		}
	}
}
