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
		[SerializeField] public string CustomKeyName = string.Empty;
		public override object GetValue(NodePort port) {
			if (port.fieldName == "aiKeyCmd")
				return aiKeyCmd;
			return null;
		}

		public override string ToCondString(string luaPlayer)
		{
			string cmd = string.Empty;
			if (aiKeyCmd == null)
				cmd = CustomKeyName;
			else
				cmd = aiKeyCmd.name;
			if (string.IsNullOrEmpty (cmd))
				return string.Empty;
			string ret = string.Format ("trigger:Command({0}, \"{1}\")", luaPlayer, cmd);
			if (isNot)
				ret = "not " + ret;
			return ret;
		}

		public override void OnRemoveConnection(NodePort port)
		{
			base.OnRemoveConnection(port);
			DoDisConnect<AI_KeyCmd>(port, ref aiKeyCmd);
		}

		public override void OnCreateConnection(NodePort from, NodePort to)
		{
			base.OnCreateConnection (from, to);
			DoCreateConnect<AI_KeyCmd>(from, to, ref aiKeyCmd, "aiKeyCmd");
		}


		[SerializeField] public bool isNot = false;
	}
}