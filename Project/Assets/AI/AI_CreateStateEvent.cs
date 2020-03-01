using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	//[CreateNodeMenu("AI/创建StateEvent")]
	[Serializable]
	public abstract class AI_CreateStateEvent: AI_BaseNode
	{
		public static bool VaildAnim(int anim)
		{
			return anim != CNSStateDef._cNoVaildAnim;
		}

		public static string VaildStr(float v, bool hasPer)
		{
			if (Mathf.Abs (v - CNSStateDef._cNoVaildVelset) <= float.Epsilon)
				return "nil";
			else {
				if (hasPer)
					return v.ToString () + " * VelSetPer";
				return v.ToString ();
			}
		}

		public static string VaildStr(int v)
		{
			if (v == CNSStateDef._cNoVaildVelset)
				return "nil";
			else
				return v.ToString ();
		}

		[Input(ShowBackingValue.Never, ConnectionType.Override)]
		[SerializeField] public AI_CreateStateDef parent;

		[Input(ShowBackingValue.Never, ConnectionType.Override)]
		[SerializeField] public AI_BaseCondition condition;

		[SerializeField] public bool setPersistent = false;

		[SerializeField] public CnsStateTriggerType triggleType = CnsStateTriggerType.AnimElem;

		[SerializeField] public bool isVisible = true;

		public override void OnRemoveConnection(NodePort port)
		{
			base.OnRemoveConnection(port);

			if (port.direction != NodePort.IO.Input)
				return;

			if (port.ValueType == typeof(AI_CreateStateDef)) {
				DoDisConnect<AI_CreateStateDef> (port, ref parent);
			} else if (port.ValueType == typeof(AI_BaseCondition)) {
				DoDisConnect<AI_BaseCondition> (port, ref condition);
			}
		}

		public override void OnCreateConnection(NodePort from, NodePort to)
		{
			base.OnCreateConnection (from, to);

			if (from.node != this) {
				if (from != null && (from.node.GetType () == typeof(AI_Cond_PlayerTime) ||
					from.node.GetType () == typeof(AI_Cond_PlayerAniTime))) {
					triggleType = CnsStateTriggerType.AnimTime;
				} else if ((from != null) && (from.node.GetType () == typeof(AI_Cond_PlayerAniElem)) ||
					(from.node.GetType () == typeof(AI_Cond_PlayerAnimElemTime))) {
					triggleType = CnsStateTriggerType.AnimElem;
				}

				if (from.node is AI_CreateStateDef)
					DoCreateConnect (from, to, ref parent, "parent");
				else if (from.node is AI_BaseCondition)
					DoCreateConnect (from, to, ref condition, "condition");
				else {
					var p1 = GetPort ("parent");
					var p2 = GetPort ("condition");
					from.Disconnect (p1);
					from.Disconnect (p2);
				}
			}


		}

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "parent")
				return parent;
			else if (port.fieldName == "condition")
				return condition;
			return null;
		}

		protected virtual string GetDoStr(bool hasCond)
		{
			return string.Empty;
		}

		public override string ToString ()
		{
			string ret = string.Empty;

			if (!isVisible)
				return ret;

			string condStr = string.Empty;
			if (condition != null) {
				condStr = condition.ToCondString ("luaPlayer");
			}
			string doStr = GetDoStr (!string.IsNullOrEmpty(condStr));
			if (!string.IsNullOrEmpty (doStr)) {
				ret = string.Format ("\t\tlocal state = def:CreateStateEvent({0}.{1})\n\r", triggleType.GetType ().FullName, triggleType.ToString ());
				ret += string.Format ("\t\tstate.OnTriggerEvent = \n\r");
				ret += string.Format ("\t\t\t\tfunction (luaPlayer, state)\n\r");


				bool isCond = false;
				if (!string.IsNullOrEmpty (condStr)) {
					ret += "\t\t\t\t\t\tlocal trigger1 = (" + condStr + ")\n\r";
					ret += "\t\t\t\t\t\tif trigger1 then\n\r";
					ret += "\t\t";
					isCond = true;
				}

				ret += "\t\t\t\t\t\t" + doStr + "\n\r";

				if (setPersistent) {
					if (isCond)
						ret += "\t\t";
					ret += "\t\t\t\t\t\ttrigger:Persistent(luaPlayer, state, true)\n\r";
				}

				if (isCond) {
					ret += "\t\t\t\t\t\tend\n\r";
				}

				ret += "\t\t\t\tend\n\r";
			}

			return ret;
		}
	}
}
