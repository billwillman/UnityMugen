using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateDef")]
	[Serializable]
	public class AI_CreateStateDef: AI_BaseNode
	{

		[SerializeField] public int realAnimate = CNSStateDef._cNoVaildAnim;

		public string Animate
		{
			get {
				string ret = string.Empty;

                if (input != null && input.aiType == AI_Type.ChangeState) {
                    ret = input.value;
                } else if (prevCns != null) {
                    ret = prevCns.Animate;
                } else if (realAnimate != CNSStateDef._cNoVaildAnim)
                    return realAnimate.ToString();

                return ret;
			}
		}

		public override string ToString ()
		{
			string ret = string.Empty;
			string animate = this.Animate;
			if (string.IsNullOrEmpty (animate))
				return ret;

			ret = string.Format ("\t\tlocal id = luaCfg:CreateStateDef(\"{0}\")\n\r", animate);
			ret += "\t\tlocal def = luaCfg:GetStateDef(id)\n\r";
			if (type != Cns_Type.none)
				ret += string.Format ("\t\tdef.Type = {0}.{1}\n\r", type.GetType ().FullName, type.ToString ());
			if (physicsType != Cns_PhysicsType.none)
				ret += string.Format ("\t\tdef.PhysicsType = {0}.{1}\n\r", physicsType.GetType ().FullName, physicsType.ToString ());
			if (moveType != Cns_MoveType.none)
				ret += string.Format ("\t\tdef.MoveType = {0}.{1}\n\r", moveType.GetType ().FullName, moveType.ToString ());
			ret += string.Format ("\t\tdef.Juggle = {0:D}\n\r", juggle);
			ret += string.Format ("\t\tdef.PowerAdd = {0:D}\n\r", powerAdd);

			if (Mathf.Abs (velset_x - CNSStateDef._cNoVaildVelset) > float.Epsilon) {
				ret += string.Format ("\t\tdef.Velset_x = {0}\n\r", velset_x.ToString ());
			}

			if (Mathf.Abs (velset_y - CNSStateDef._cNoVaildVelset) > float.Epsilon) {
				ret += string.Format ("\t\tdef.Velset_y = {0}\n\r", velset_y.ToString ());
			}

			ret += string.Format ("\t\tdef.Ctrl = {0:D}\n\r", ctrl);
			ret += string.Format ("\t\tdef.Sprpriority = {0:D}\n\r", sprpriority);

			if (realAnimate != CNSStateDef._cNoVaildAnim) {
				ret += string.Format ("\t\tdef.Animate = {0:D}\n\r", realAnimate);
			} else {
				if (!string.IsNullOrEmpty (animate))
					ret += string.Format ("\t\tdef.Animate = {0}\n\r", animate);
			}

			// 创建State
			var pp = this.GetPort("output");
			if (pp != null) {
				for (int i = 0; i < pp.ConnectionCount; ++i) {
					var n = pp.GetConnection (i);
					if (n == null || n.node == null)
						continue;
					if (n.node is AI_CreateStateEvent) {
						ret += n.node.ToString ();
					}
				}
			}

			return ret;
		}

		[Input(ShowBackingValue.Never, ConnectionType.Override)]
		[SerializeField] public AI_Cmd input;
		[Input(ShowBackingValue.Never, ConnectionType.Override)]
		[SerializeField] public AI_StateEvent_PlayCns prevCns;
		[Output(ShowBackingValue.Never)]
		[SerializeField] public AI_CreateStateDef output;

		[SerializeField] public Cns_Type type = Cns_Type.S;
		[SerializeField] public Cns_MoveType moveType = Cns_MoveType.none;
		[SerializeField] public Cns_PhysicsType physicsType = Cns_PhysicsType.S;
		[SerializeField] public int juggle = 0;
		[SerializeField] public int powerAdd = 0;
		[SerializeField] public float velset_x = CNSStateDef._cNoVaildVelset;
		[SerializeField] public float velset_y = CNSStateDef._cNoVaildVelset;
		//public int animate = 0;
		[SerializeField] public int ctrl = CNSStateDef._cNoVaildCtrl;
		[SerializeField] public int sprpriority = 0;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "input")
				return input;
			else if (port.fieldName == "output")
				return output;
			else if (port.fieldName == "prevCns")
				return prevCns;
			return null;
		}

		public override void OnCreateConnection(NodePort from, NodePort to) { 
			base.OnCreateConnection (from, to);

			if (from != null && from.node != this) {
				if (from != null && from.node.GetType () == typeof(AI_Cmd)) {
					if (DoCreateConnect<AI_Cmd> (from, to, ref input, "input")) {
						var pp = GetPort ("prevCns");
						if (pp != null)
							pp.ClearConnections ();
					}
				} else if (from != null && from.node.GetType () == typeof(AI_StateEvent_PlayCns)) {
					if (DoCreateConnect<AI_StateEvent_PlayCns> (from, to, ref prevCns, "prevCns")) {
						var pp = GetPort ("input");
						if (pp != null)
							pp.ClearConnections ();
					}
				} else {
					from.Disconnect (to);
				}
			}
		}

		public override void OnRemoveConnection(NodePort port)
		{
			base.OnRemoveConnection(port);

			if (port.direction != NodePort.IO.Input) {
				return;
			}

			if (port.ValueType == typeof(AI_Cmd)) {
				DoDisConnect<AI_Cmd> (port, ref input);
			} else if (port.ValueType == typeof(AI_StateEvent_PlayCns)) {
				DoDisConnect<AI_StateEvent_PlayCns> (port, ref prevCns);
			}
		}
	}
}
