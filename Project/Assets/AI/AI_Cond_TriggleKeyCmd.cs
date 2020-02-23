using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/条件/触发按键")]
	[Serializable]
	public class AI_Cond_TriggleKeyCmd: AI_BaseCondition
	{
		[Input(ShowBackingValue.Never, ConnectionType.Override)]
		[SerializeField] public AI_KeyCmd aiKeyCmd;
		public override object GetValue(NodePort port) {
			if (port.fieldName == "aiKeyCmd")
				return aiKeyCmd;
			return null;
		}

		public override string ToCondString(string luaPlayer)
		{
			if (aiKeyCmd == null)
				return string.Empty;
			string ret = string.Format ("trigger:Command({0}, \"{1}\")", luaPlayer, aiKeyCmd.name);
			if (isNot)
				ret = "not " + ret;
			return ret;
		}

		public override void OnRemoveConnection(NodePort port)
		{
			DoDisConnect<AI_KeyCmd>(port, ref aiKeyCmd);
		}

		public override void OnCreateConnection(NodePort from, NodePort to)
		{
			DoCreateConnect<AI_KeyCmd>(from, to, ref aiKeyCmd, "aiKeyCmd");
		}


		[SerializeField] public bool isNot = false;
	}
}