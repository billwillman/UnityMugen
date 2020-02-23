using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/设置速度")]
	[Serializable]
	public class AI_StateEvent_VelSet: AI_CreateStateEvent
	{
		[SerializeField] public float velx = CNSStateDef._cNoVaildVelset;
		[SerializeField] public float vely = CNSStateDef._cNoVaildVelset;

		protected override string GetDoStr(bool hasCond)
		{
			string velXStr = VaildStr (velx, true);
			string velYStr = VaildStr (vely, true);

			if (velXStr == "nil" && velYStr == "nil")
				return string.Empty;

			string ret = string.Format ("trigger:VelSet(luaPlayer, {0}, {1})", velXStr, velYStr);
			return ret;
		}
	}
}
