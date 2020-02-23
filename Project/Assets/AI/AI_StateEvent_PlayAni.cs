using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/播放动画")]
	[Serializable]
	public class AI_StateEvent_PlayAni: AI_CreateStateEvent
	{
		[SerializeField] public int anim = CNSStateDef._cNoVaildAnim;
		[SerializeField] public bool isLoop = false;
		//public bool isCleanStateDef = true;

		protected override string GetDoStr(bool hasCond)
		{
			if (!VaildAnim(anim))
				return string.Empty;
			string ret = string.Format ("trigger:PlayAnim(luaPlayer, {0:D}, {1})", anim, isLoop.ToString().ToLower());
			return ret;
		}
	}
}
