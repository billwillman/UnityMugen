using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/删除爆炸物")]
	[Serializable]
	public class AI_StateEvent_RemoveExplod: AI_CreateStateEvent
	{
		[SerializeField] public int id = -1;

		protected override string GetDoStr(bool hasCond)
		{
			if (id >= 0) {
				string ret = string.Format ("trigger:RemoveExplod(luaPlayer, {0:D})", id);
				return ret;
			}
			return string.Empty;
		}
	}
}
