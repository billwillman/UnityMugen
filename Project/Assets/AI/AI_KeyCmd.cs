using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/按键设置")]
	[Serializable]
	public class AI_KeyCmd : AI_BaseCondition {
		[SerializeField] public string name = "KeyCmd_Unknown";
		[SerializeField] public float time = 1;
		[SerializeField] public string keyCommands;

		public override string ToCondString(string luaPlayer)
		{
			if (string.IsNullOrEmpty(name))
				return string.Empty;
			string ret = string.Format ("trigger:Command({0}, \"{1}\")", luaPlayer, name);
			return ret;
		}
	}
}
