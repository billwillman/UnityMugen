using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/条件/Or")]
	[Serializable]
	public class AI_Cond_Or: AI_BaseCondition
	{
		[Input(ShowBackingValue.Never)]
		[SerializeField] public List<AI_BaseCondition> inputs;
		public override object GetValue(NodePort port) {
			return inputs;
		}

		public override void OnRemoveConnection(NodePort port)
		{
			DoDisConnectToList (port, ref inputs);
		}

		public override void OnCreateConnection(NodePort from, NodePort to)
		{
			DoCreateConnectToList (from, to, ref inputs, "inputs");
		}

		public override string ToCondString(string luaPlayer)
		{
			if (inputs == null || inputs.Count <= 0)
				return string.Empty;
			string ret = string.Empty;
			bool isFirst = true;
			for (int i = 0; i < inputs.Count; ++i) {
				var input = inputs [i];
				if (input == null)
					continue;
				if (isFirst) {
					string str = input.ToCondString (luaPlayer);
					if (!string.IsNullOrEmpty (str)) {
						ret = string.Format ("({0})", str);
						isFirst = false;
					}
				} else {
					string str = input.ToCondString (luaPlayer);
					if (!string.IsNullOrEmpty (str)) {
						ret += string.Format (" or ({0})", str);
					}
				}
			}

			return ret;
		}
	}
}
