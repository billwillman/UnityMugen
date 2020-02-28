using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/设置Sprpriority")]
	[Serializable]	
	public class AI_StateEvent_SetSprpriority : AI_CreateStateEvent {
		[SerializeField] public int sprpriority = 0;

		protected override string GetDoStr(bool hasCond)
		{
			string ret = string.Format ("trigger:SetSprpriority(luaPlayer, {0:D})", sprpriority);
			return ret;
		}
	}

}
