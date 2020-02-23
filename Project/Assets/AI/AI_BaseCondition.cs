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
			return this;
		}

	}
}
