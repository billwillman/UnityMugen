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
	public abstract class AI_BaseNode: Node
	{

		internal static string GetOpStr(AI_Cond_Op op)
		{
			switch (op) {
			case AI_Cond_Op.Equal:
				return " == ";
			case AI_Cond_Op.Great:
				return " > ";
			case AI_Cond_Op.GreatAndEqual:
				return " >= ";
			case AI_Cond_Op.Less:
				return " < ";
			case AI_Cond_Op.LessOrEqual:
				return " <= ";
			case AI_Cond_Op.NotEqual:
				return " ~= ";
			}
			return string.Empty;
		}

		protected bool DoCreateConnect<T>(NodePort from, ref T item, string itemName, NodePort.IO dir = NodePort.IO.Input) where T: Node
		{
			if (dir == NodePort.IO.Input) {
				if (from.node == this)
					return false;
			} else if (dir == NodePort.IO.Output) {
				if (from.node != this)
					return false;
			}
			//if (from.node.GetType ().IsSubclassOf (typeof(T))) {
			item =  from.node as T;
			//}
			if (item == null) {
				var port = GetInputPort (itemName);
				if (port != null) {
					port.Disconnect (from);
				}
				return false;
			}
			return true;
		}

		protected bool DoCreateConnectToList<T>(NodePort from, ref List<T> condList, string condListName, NodePort.IO dir = NodePort.IO.Input) where T: Node
		{
			if (dir == NodePort.IO.Input) {
				if (from.node == this)
					return false;
			} else if (dir == NodePort.IO.Output) {
				if (from.node != this)
					return false;
			}

			if (from.node is T) {
				if (condList == null)
					condList = new List<T> ();
				T cc = from.node as T;	
				if (!condList.Contains (cc)) {
					condList.Add (cc);
				}
				return true;
			} else {
				var port = GetInputPort (condListName);
				int idx = port.GetConnectionIndex (from);
				if (idx >= 0)
					port.Disconnect (idx);

				return false;
			}

			return false;
		}

		protected bool DoDisConnect<T>(NodePort port, ref T item, NodePort.IO dir = NodePort.IO.Input) where T: Node
		{
			if (port.direction != dir)
				return false;

			for (int i = 0; i < port.ConnectionCount; ++i) {
				T cc = port.GetConnection(i).node as T;
				if (cc != null) {
					item = cc;
					break;
				}
			}
			item = null;
			return true;
		}

		protected bool DoDisConnectToList<T>(NodePort port, ref List<T> condList, NodePort.IO dir = NodePort.IO.Input) where T: Node
		{
			if (port.direction != dir)
				return false;

			if (condList != null)
				condList.Clear ();
			else
				condList = new List<T> ();

			for (int i = 0; i < port.ConnectionCount; ++i) {
				T cc = port.GetConnection(i).node as T;
				if (cc != null) {
					condList.Add (cc);
				}
			}

			return true;
		}
	}
}
