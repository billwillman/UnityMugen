using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/速度增加")]
	[Serializable]
	public class AI_StateEvent_VelAdd: AI_CreateStateEvent
	{
		[SerializeField] public float velx = CNSStateDef._cNoVaildVelset;
		[SerializeField] public float vely = CNSStateDef._cNoVaildVelset;

		protected override string GetDoStr(bool hasCond)
		{
			string velXStr = VaildStr (velx, false);
			string velYStr = VaildStr (vely, false);

			if (velXStr == "nil" && velYStr == "nil")
				return string.Empty;

			string ret = string.Format ("trigger:VelAdd(luaPlayer, {0}, {1})", velXStr, velYStr);
			return ret;
		}
	}
}
