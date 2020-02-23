using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/角色暂停")]
	[Serializable]
	public class AI_StateEvent_PlayerPause: AI_CreateStateEvent
	{
		[SerializeField] public float time;

		protected override string GetDoStr(bool hasCond)
		{
			string ret = string.Format ("trigger:Pause(luaPlayer, {0})", time.ToString());
			return ret;
		}
	}
}
