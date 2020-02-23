using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/切换Stand状态")]
	[Serializable]
	public class AI_StateEvent_PlayStandCns: AI_CreateStateEvent
	{
		[SerializeField] public bool IsSetCtrl_1 = true;

		protected override string GetDoStr(bool hasCond)
		{
			string ret = "trigger:PlayStandCns(luaPlayer)";

			if (IsSetCtrl_1) {
				string pre = string.Empty;
				if (hasCond)
					pre = "\t\t\t\t\t\t\t\t";
				else
					pre = "\t\t\t\t\t\t";
				ret  += "\n\r" + pre + "trigger:CtrlSet(luaPlayer, 1)\n\r";
			}

			return ret;
		}
	}

}