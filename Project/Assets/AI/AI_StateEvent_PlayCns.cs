using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/切换Cns状态")]
	[Serializable]
	public class AI_StateEvent_PlayCns: AI_CreateStateEvent
	{
		[Output(ShowBackingValue.Never, ConnectionType.Override)]
		[SerializeField] public AI_CreateStateDef nextStateDef;

		[SerializeField] public string Animate;
		[SerializeField] public bool IsLoop = false;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "nextStateDef")
				return nextStateDef;
			return null;
		}

		protected override string GetDoStr(bool hasCond)
		{
			string ret = string.Empty;
			if (!string.IsNullOrEmpty (Animate)) {
				ret = string.Format ("trigger:PlayCnsByName(luaPlayer, {0}, {1})", Animate, IsLoop.ToString ().ToLower());
			}
			return ret;
		}
	}
}
