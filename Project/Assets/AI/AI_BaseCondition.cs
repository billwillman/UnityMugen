using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[Serializable]
	public abstract class AI_BaseCondition : AI_BaseNode
	{

		public virtual string ToCondString(string luaPlayer)
		{
			return string.Empty;
		}

		[Output]
		[SerializeField] public AI_BaseCondition output;
		public override object GetValue(NodePort port) {
			if (port.fieldName == "output") {
				return this;
			} else
				return null;
		}

		public override void OnCreateConnection(NodePort from, NodePort to)
		{
			base.OnCreateConnection (from, to);
			DoCreateConnect(from, to, ref output, "output", NodePort.IO.Output);
		}

		public override void OnRemoveConnection(NodePort port) 
		{
			base.OnRemoveConnection(port);
			DoDisConnect(port, ref output, NodePort.IO.Output);
		}

	}
}
