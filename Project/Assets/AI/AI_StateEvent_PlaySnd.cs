using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/播放声音")]
	[Serializable]
	public class AI_StateEvent_PlaySnd: AI_CreateStateEvent
	{
		[SerializeField] public int group;
		[SerializeField] public int index;

		protected override string GetDoStr(bool hasCond)
		{
			string ret = string.Format ("trigger:PlaySnd(luaPlayer, {0:D}, {1:D})", group, index);
			return ret;
		}
	}

}
