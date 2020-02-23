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

		protected bool DoCreateConnect<T>(NodePort from, NodePort to, ref T item, string itemName, NodePort.IO dir = NodePort.IO.Input) where T: Node
		{
			// 不允许自己连接自己
			if (from.node == to.node) {
				from.Disconnect (to);
				return false;
			}

			if (dir == NodePort.IO.Input) {
				if (from.node == this)
					return false;
				
			} else if (dir == NodePort.IO.Output) {
				if (from.node != this)
					return false;
			} else
				return false;

			if (dir == NodePort.IO.Input) {
				if (to.fieldName != itemName)
					return false;
				item =  from.node as T;
			} else if (dir == NodePort.IO.Output) {
				if (from.fieldName != itemName)
					return false;
				item = to.node as T;
			} else
				return false;

			if (item == null) {
				from.Disconnect (to);
				return false;
			}
			return true;
		}

		protected bool DoCreateConnectToList<T>(NodePort from, NodePort to, ref List<T> condList, string condListName, NodePort.IO dir = NodePort.IO.Input) where T: Node
		{

			// 不允许自己连接自己
			if (from.node == to.node) {
				from.Disconnect (to);
				return false;
			}
				
			if (dir == NodePort.IO.Input) {
				if (from.node == this)
					return false;
			} else if (dir == NodePort.IO.Output) {
				if (from.node != this)
					return false;
			} else
				return false;

			T item = default(T);
			if (dir == NodePort.IO.Input) {
				if (to.fieldName != condListName)
					return false;
				item =  from.node as T;
			} else if (dir == NodePort.IO.Output) {
				if (from.fieldName != condListName)
					return false;
				item = to.node as T;
			} else
				return false;

			if (item != null) {
				if (condList == null)
					condList = new List<T> ();
				T cc = from.node as T;	
				if (!condList.Contains (cc)) {
					condList.Add (cc);
				}
				return true;
			} else {
				from.Disconnect (to);
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
