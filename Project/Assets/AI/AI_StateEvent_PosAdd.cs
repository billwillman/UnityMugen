using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/位置增加")]
	[Serializable]
	public class AI_StateEvent_PosAdd: AI_CreateStateEvent
	{
		[SerializeField] public int x = CNSStateDef._cNoVaildVelset;
		[SerializeField] public int y = CNSStateDef._cNoVaildVelset;

		protected override string GetDoStr(bool hasCond)
		{
			string velXStr = VaildStr (x);
			string velYStr = VaildStr (y);

			if (velXStr == "nil" && velYStr == "nil")
				return string.Empty;

			string ret = string.Format ("trigger:PosAdd(luaPlayer, {0}, {1})", VaildStr (x), VaildStr (y));
			return ret;
		}
	}
}
