using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/速度乘法")]
	[Serializable]
	public class AI_StateEvent_VelMul: AI_CreateStateEvent
	{
		[SerializeField] public float velX = CNSStateDef._cNoVaildVelset;
		[SerializeField] public float velY = CNSStateDef._cNoVaildVelset;

		protected override string GetDoStr(bool hasCond)
		{
			string velXStr = VaildStr (velX, false);
			string velYStr = VaildStr (velY, false);
			if (velXStr == "nil" && velYStr == "nil")
				return string.Empty;
			string ret = string.Format ("trigger:VelMul(luaPlayer, {0}, {1})", velXStr, velYStr);
			return ret;
		}
	}
}
