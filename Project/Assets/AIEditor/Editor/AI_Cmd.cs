using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;

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

[CreateNodeMenu("AI/按键设置")]
public class AI_KeyCmd : AI_BaseCondition {
	public string name = "KeyCmd_Unknown";
	public float time = 1;
	public string keyCommands;

	public override string ToCondString(string luaPlayer)
	{
		if (string.IsNullOrEmpty(name))
			return string.Empty;
		string ret = string.Format ("trigger:Command({0}, \"{1}\")", luaPlayer, name);
		return ret;
	}
}


[CreateNodeMenu("AI/AI命令")]
public class AI_Cmd : AI_BaseNode
{
	
	public string cmdName = "AICmd_Unknown";

	public AI_Type aiType = AI_Type.ChangeState;

	public string value;

	[Input(ShowBackingValue.Never)]
	public List<AI_BaseCondition> condList;

	[Output(ShowBackingValue.Never)]
	public AI_Cmd output;

	public override object GetValue(NodePort port) {
		if (port.fieldName == "condList")
			return condList;
		else if (port.fieldName == "output")
			return output;
		return null;
	}

	public override void OnRemoveConnection(NodePort port)
	{
		DoDisConnectToList<AI_BaseCondition> (port, ref condList);
	}

	public override void OnCreateConnection(NodePort from, NodePort to)
	{
		DoCreateConnectToList<AI_BaseCondition> (from, ref condList, "condList");
	}
}

public abstract class AI_BaseCondition : AI_BaseNode
{

	public virtual string ToCondString(string luaPlayer)
	{
		return string.Empty;
	}

	[Output]
	public AI_BaseCondition output;
	public override object GetValue(NodePort port) {
		return this;
	}
		
}

[CreateNodeMenu("AI/条件/And")]
public class AI_Cond_And: AI_BaseCondition
{
	[Input(ShowBackingValue.Never)]
	public List<AI_BaseCondition> inputs;
	public override object GetValue(NodePort port) {
		return inputs;
	}

	public override void OnRemoveConnection(NodePort port)
	{
		DoDisConnectToList (port, ref inputs);
	}

	public override void OnCreateConnection(NodePort from, NodePort to)
	{
		DoCreateConnectToList (from, ref inputs, "inputs");
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
					ret += string.Format (" and ({0})", str);
				}
			}
		}

		return ret;
	}

}

[CreateNodeMenu("AI/条件/Or")]
public class AI_Cond_Or: AI_BaseCondition
{
	[Input(ShowBackingValue.Never)]
	public List<AI_BaseCondition> inputs;
	public override object GetValue(NodePort port) {
		return inputs;
	}

	public override void OnRemoveConnection(NodePort port)
	{
		DoDisConnectToList (port, ref inputs);
	}

	public override void OnCreateConnection(NodePort from, NodePort to)
	{
		DoCreateConnectToList (from, ref inputs, "inputs");
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

[CreateNodeMenu("AI/条件/触发按键")]
public class AI_Cond_TriggleKeyCmd: AI_BaseCondition
{
	[Input(ShowBackingValue.Never, ConnectionType.Override)]
	public AI_KeyCmd aiKeyCmd;
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
		DoCreateConnect<AI_KeyCmd>(from, ref aiKeyCmd, "aiKeyCmd");
	}


	public bool isNot = false;
}

[CreateNodeMenu("AI/条件/角色StateType状态")]
public class AI_Cond_PlayerStateType: AI_BaseCondition
{
	public Cns_Type stateType = Cns_Type.S;

	public bool isNot = false;	

	public override string ToCondString(string luaPlayer)
	{
		string ret = string.Format ("{0}.{1}", stateType.GetType ().FullName, stateType.ToString ());
		if (isNot)
			ret = "trigger:Statetype ~= " + ret;
		else
			ret = "trigger:Statetype == " + ret;
		return ret;
	}
}

[CreateNodeMenu("AI/条件/角色Ctrl状态")]
public class AI_Cond_PlayerCtrl: AI_BaseCondition
{
	public int Ctrl = 1;
	public bool isNot = false;	

	public override string ToCondString(string luaPlayer)
	{
		string ret = string.Format ("trigger:Ctrl({0})", luaPlayer);
		if (isNot)
			ret += " ~= ";
		else
			ret += " == ";
		ret += Ctrl.ToString ();
		return ret;
	}
}

public enum AI_Cond_Op
{
	Less,
	LessOrEqual,
	Great,
	GreatAndEqual,
	Equal,
	NotEqual
}

[CreateNodeMenu("AI/条件/角色AnimElem")]
public class AI_Cond_PlayerAniElem: AI_BaseCondition
{
	public int aniElem;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:AnimElem({0}){1}{2:D}", luaPlayer, opStr, aniElem);
		return ret;
	}
}

[CreateNodeMenu("AI/条件/角色AnimTime")]
public class AI_Cond_PlayerAniTime: AI_BaseCondition
{
	public int aniTime;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:AnimTime({0}){1}{2:D}", luaPlayer, opStr, aniTime);
		return ret;
	}
}

[CreateNodeMenu("AI/条件/角色Time")]
public class AI_Cond_PlayerTime: AI_BaseCondition
{
	public int time;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:Time({0}){1}{2:D}", luaPlayer, opStr, time);
		return ret;
	}
}

[CreateNodeMenu("AI/条件/角色 VelX")]
public class AI_Cond_Player_VelX: AI_BaseCondition
{
	public float velX;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:VelX({0}){1}{2}", luaPlayer, opStr, velX.ToString());
		return ret;
	}
}

[CreateNodeMenu("AI/条件/角色 VelY")]
public class AI_Cond_Player_VelY: AI_BaseCondition
{
	public float velY;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:VelY({0}){1}{2}", luaPlayer, opStr, velY.ToString());
		return ret;
	}
}

[CreateNodeMenu("AI/条件/角色 PosX")]
public class AI_Cond_Player_PosX: AI_BaseCondition
{
	public float posX;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:PosX({0}){1}{2}", luaPlayer, opStr, posX.ToString());
		return ret;
	}
}

[CreateNodeMenu("AI/条件/角色 PosY")]
public class AI_Cond_Player_PosY: AI_BaseCondition
{
	public float posY;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:PosY({0}){1}{2}", luaPlayer, opStr, posY.ToString());
		return ret;
	}
}

[CreateNodeMenu("AI/条件/角色StateNo")]
public class AI_Cond_Player_StateNo: AI_BaseCondition
{
	public int stateNo;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:Stateno({0}){1}{2:D}", luaPlayer, opStr, stateNo);
		return ret;
	}
}

[CreateNodeMenu("AI/条件/float var")]
public class AI_Cond_Player_FloatVar: AI_BaseCondition
{
	public int index;
	public float value;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:fVar({0}, {1:D}){2}{3}", luaPlayer, index, opStr, value.ToString());
		return ret;
	}
}

[CreateNodeMenu("AI/条件/int var")]
public class AI_Cond_Player_IntVar: AI_BaseCondition
{
	public int index;
	public int value;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:Var({0}, {1:D}){2}{3:D}", luaPlayer, index, opStr, value);
		return ret;
	}
}

[CreateNodeMenu("AI/条件/角色PrevStateNo")]
public class AI_Cond_Player_PlayerPrevStateNo: AI_BaseCondition
{
	public int prevStateNo;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:PrevStateNo({0}){1}{2:D}", luaPlayer, opStr, prevStateNo);
		return ret;
	}
}

/*
[CreateNodeMenu("AI/条件/角色状态Persistent")]
public class AI_Cond_PlayerState_Persistent: AI_BaseCondition
{
	public bool isNot = false;

	public override string ToCondString(string luaPlayer)
	{
		string ret = string.Format ("trigger:IsPersistent({0}, state)", luaPlayer);
		if (isNot)
			ret = "not " + ret;
	}
}*/

[CreateNodeMenu("AI/条件/角色Power值")]
public class AI_Cond_PlayerPower: AI_BaseCondition
{
	public int value;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:Power({0}){1}{2:D}", luaPlayer, opStr, value);
		return ret;
	}
}

[CreateNodeMenu("AI/条件/角色Life值")]
public class AI_Cond_PlayerLife: AI_BaseCondition
{
	public int value;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:Life({0}){1}{2:D}", luaPlayer, opStr, value);
		return ret;
	}
}

[CreateNodeMenu("AI/条件/角色HitCount值")]
public class AI_Cond_PlayerHitCount: AI_BaseCondition
{
	public int value;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:HitCount({0}){1}{2:D}", luaPlayer, opStr, value);
		return ret;
	}
}

[CreateNodeMenu("AI/条件/角色动画Ani是否存在")]
public class AI_Cond_PlayerAniExist: AI_BaseCondition
{
	public int value;
	public bool isNot = false;

	public override string ToCondString(string luaPlayer)
	{
		string ret = string.Format ("trigger:AnimExist({0}, {1:D})", luaPlayer, value);
		if (isNot)
			ret = "not " + ret;
		return ret;
	}
}

[CreateNodeMenu("AI/条件/角色是否Alive")]
public class AI_Cond_PlayerIsAlive: AI_BaseCondition
{
	public bool isNot = false;

	public override string ToCondString(string luaPlayer)
	{
		string ret = string.Format ("trigger:Alive({0})", luaPlayer);
		if (isNot)
			ret = "not " + ret;
		return ret;
	}
}

[CreateNodeMenu("AI/条件/角色AnimElemTime")]
public class AI_Cond_PlayerAnimElemTime: AI_BaseCondition
{
	public int frameNo;
	public int animElem;
	public AI_Cond_Op op = AI_Cond_Op.Equal;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:AnimElemTime({0}, {1:D}){2}{3:D}", luaPlayer, frameNo, opStr, animElem);
		return ret;
	}
}

[CreateNodeMenu("AI/条件/Random")]
public class AI_Cond_Random: AI_BaseCondition
{
	public int randomValue = 1000;
	public int value = 500;
	public AI_Cond_Op op = AI_Cond_Op.Less;

	public override string ToCondString(string luaPlayer)
	{
		var opStr = GetOpStr (op);
		string ret = string.Format ("trigger:Random({0:D}){1}{2:D}", randomValue, opStr, value);
		return ret;
	}
}

[CreateNodeMenu("AI/创建StateDef")]
public class AI_CreateStateDef: AI_BaseNode
{

	public int realAnimate = CNSStateDef._cNoVaildAnim;

	public string Animate
	{
		get {
			string ret = string.Empty;

			if (input != null && input.aiType == AI_Type.ChangeState) {
				ret = input.value;
			} else if (prevCns != null) {
				ret = prevCns.Animate;
			}

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
	public AI_Cmd input;
	[Input(ShowBackingValue.Never, ConnectionType.Override)]
	public AI_StateEvent_PlayCns prevCns;
	[Output(ShowBackingValue.Never)]
	public AI_CreateStateDef output;

	public Cns_Type type = Cns_Type.S;
	public Cns_MoveType moveType = Cns_MoveType.none;
	public Cns_PhysicsType physicsType = Cns_PhysicsType.S;
	public int juggle = 0;
	public int powerAdd = 0;
	public float velset_x = CNSStateDef._cNoVaildVelset;
	public float velset_y = CNSStateDef._cNoVaildVelset;
	//public int animate = 0;
	public int ctrl = 0;
	public int sprpriority = 0;

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

		if (from != null && from.node != this) {
			if (from != null && from.node.GetType () == typeof(AI_Cmd)) {
				if (DoCreateConnect<AI_Cmd> (from, ref input, "input")) {
					var pp = GetPort ("prevCns");
					if (pp != null)
						pp.ClearConnections ();
				}
			} else if (from != null && from.node.GetType () == typeof(AI_StateEvent_PlayCns)) {
				if (DoCreateConnect<AI_StateEvent_PlayCns> (from, ref prevCns, "prevCns")) {
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

//[CreateNodeMenu("AI/创建StateEvent")]
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
	public AI_CreateStateDef parent;

	[Input(ShowBackingValue.Never, ConnectionType.Override)]
	public AI_BaseCondition condition;

	public bool setPersistent = false;

	public CnsStateTriggerType triggleType = CnsStateTriggerType.AnimElem;

	public override void OnRemoveConnection(NodePort port)
	{
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
		if (from.node != this) {
			if (from != null && (from.node.GetType () == typeof(AI_Cond_PlayerTime) ||
			   from.node.GetType () == typeof(AI_Cond_PlayerAniTime))) {
				triggleType = CnsStateTriggerType.AnimTime;
			} else if ((from != null) && (from.node.GetType () == typeof(AI_Cond_PlayerAniElem)) ||
			          (from.node.GetType () == typeof(AI_Cond_PlayerAnimElemTime))) {
				triggleType = CnsStateTriggerType.AnimElem;
			}

			if (from.node is AI_CreateStateDef)
				DoCreateConnect (from, ref parent, "parent");
			else if (from.node is AI_BaseCondition)
				DoCreateConnect (from, ref condition, "condition");
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

[CreateNodeMenu("AI/创建StateEvent/播放声音")]
public class AI_StateEvent_PlaySnd: AI_CreateStateEvent
{
	public int group;
	public int index;

	protected override string GetDoStr(bool hasCond)
	{
		string ret = string.Format ("trigger:PlaySnd(luaPlayer, {0:D}, {1:D})", group, index);
		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/设置速度")]
public class AI_StateEvent_VelSet: AI_CreateStateEvent
{
	public float velx = CNSStateDef._cNoVaildVelset;
	public float vely = CNSStateDef._cNoVaildVelset;

	protected override string GetDoStr(bool hasCond)
	{
		string velXStr = VaildStr (velx, true);
		string velYStr = VaildStr (vely, true);

		if (velXStr == "nil" && velYStr == "nil")
			return string.Empty;

		string ret = string.Format ("trigger:VelSet(luaPlayer, {0}, {1})", velXStr, velYStr);
		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/速度增加")]
public class AI_StateEvent_VelAdd: AI_CreateStateEvent
{
	public float velx = CNSStateDef._cNoVaildVelset;
	public float vely = CNSStateDef._cNoVaildVelset;

	protected override string GetDoStr(bool hasCond)
	{
		string velXStr = VaildStr (velx, false);
		string velYStr = VaildStr (vely, false);

		if (velXStr == "nil" && velYStr == "nil")
			return string.Empty;

		string ret = string.Format ("trigger:VelAdd(luaPlayer, {0}, {1})", velXStr, velYStr);
		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/位置增加")]
public class AI_StateEvent_PosAdd: AI_CreateStateEvent
{
	public int x = CNSStateDef._cNoVaildVelset;
	public int y = CNSStateDef._cNoVaildVelset;

	protected override string GetDoStr(bool hasCond)
	{
		string velXStr = VaildStr (x);
		string velYStr = VaildStr (y);

		if (velXStr == "nil" && velYStr == "nil")
			return string.Empty;

		string ret = string.Format ("trigger:PosAdd(luaPlayer, {0}, {1})", VaildStr (x), VaildStr (y));
		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/设置位置")]
public class AI_StateEvent_PosSet: AI_CreateStateEvent
{
	public int x = CNSStateDef._cNoVaildVelset;
	public int y = CNSStateDef._cNoVaildVelset;

	protected override string GetDoStr(bool hasCond)
	{
		string velXStr = VaildStr (x);
		string velYStr = VaildStr (y);

		if (velXStr == "nil" && velYStr == "nil")
			return string.Empty;

		string ret = string.Format ("trigger:PosSet(luaPlayer, {0}, {1})", VaildStr (x), VaildStr (y));
		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/StateType设置")]
public class AI_StateEvent_StateTypeSet: AI_CreateStateEvent
{
	public Cns_Type stateType = Cns_Type.S;

	protected override string GetDoStr(bool hasCond)
	{
		string ret = string.Format ("trigger:StateTypeSet(luaPlayer, {0}.{1})", stateType.GetType().FullName, stateType.ToString());
		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/PhysicsType设置")]
public class AI_StateEvent_PhysicsTypeSet: AI_CreateStateEvent
{
	public Cns_PhysicsType physicsType = Cns_PhysicsType.none;

	protected override string GetDoStr(bool hasCond)
	{
		if (physicsType == Cns_PhysicsType.none)
			return string.Empty;
		
		string ret = string.Format ("trigger:PhysicsTypeSet(luaPlayer, {0}.{1})", physicsType.GetType().FullName, physicsType.ToString());
		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/Ctrl设置")]
public class AI_StateEvent_CtrlSet: AI_CreateStateEvent
{
	public int ctrl = 1;

	protected override string GetDoStr(bool hasCond)
	{
		string ret = string.Format ("trigger:CtrlSet(luaPlayer, {0:D})", ctrl);
		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/创建爆炸")]
public class AI_StateEvent_CreateExplod: AI_CreateStateEvent
{
	public int anim = CNSStateDef._cNoVaildAnim;
	public int id = -1;
	public int pos_x = 0;
	public int pos_y = 0;
	public ExplodPosType explodPosType = ExplodPosType.p1;
	public int bindtime = -1;
	public int removetime = -2;
	public int sprpriority = 0;
	public int removeongethit = 0;
	public int ignorehitpause = 1;
	public bool isChangeStateRemove = true;
	public bool IsUseParentUpdate = true;

	protected override string GetDoStr(bool hasCond)
	{
		string pre = string.Empty;
		if (hasCond)
			pre = "\t\t\t\t\t\t\t\t";
		else
			pre = "\t\t\t\t\t\t";
		string ret = "local explod = trigger:CreateExplod(luaPlayer)\n\r";
		if (VaildAnim (anim))
			ret += string.Format ("{0}explod.anim = {1:D}\n\r", pre, anim);
		else
			return string.Empty;

		if (id >= 0)
			ret += string.Format ("{0}explod.ID = {1:D}\n\r", pre, id);
		if (pos_x != 0)
			ret += string.Format ("{0}explod.pos_x = {1:D}\n\r", pre, pos_x);
		if (pos_y != 0)
			ret += string.Format ("{0}explod.pos_y = {1:D}\n\r", pre, pos_y);

		ret += string.Format ("{0}explod.postype = {1}.{2}\n\r", pre, explodPosType.GetType ().FullName, explodPosType.ToString ());

		if (bindtime > 0)
			ret += string.Format ("{0}explod.bindtime = {1:D} * bindTimePer\n\r", pre, bindtime);


		ret += string.Format ("{0}explod.removetime = {1:D}\n\r", pre, removetime);

		if (sprpriority != 0)
			ret += string.Format ("{0}explod.sprpriority = {1:D}\n\r", pre, sprpriority);

		ret += string.Format ("{0}explod.removeongethit = {1:D}\n\r", pre, removeongethit);
		ret += string.Format ("{0}explod.ignorehitpause = {1:D}\n\r", pre, ignorehitpause);
		ret += string.Format ("{0}explod.isChangeStateRemove = {1}\n\r", pre, isChangeStateRemove.ToString().ToLower());
		ret += string.Format ("{0}explod.IsUseParentUpdate = {1}\n\r", pre, IsUseParentUpdate.ToString().ToLower());

		ret += pre + "explod:Apply()\n\r";

		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/播放动画")]
public class AI_StateEvent_PlayAni: AI_CreateStateEvent
{
	public int anim = CNSStateDef._cNoVaildAnim;
	public bool isLoop = false;
	//public bool isCleanStateDef = true;

	protected override string GetDoStr(bool hasCond)
	{
		if (!VaildAnim(anim))
			return string.Empty;
		string ret = string.Format ("trigger:PlayAnim(luaPlayer, {0:D}, {1})", anim, isLoop.ToString().ToLower());
		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/切换Cns状态")]
public class AI_StateEvent_PlayCns: AI_CreateStateEvent
{
	[Output(ShowBackingValue.Never, ConnectionType.Override)]
	public AI_CreateStateDef nextStateDef;

	public string Animate;
	public bool IsLoop = false;

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

[CreateNodeMenu("AI/创建StateEvent/切换Stand状态")]
public class AI_StateEvent_PlayStandCns: AI_CreateStateEvent
{
	public bool IsSetCtrl_1 = true;

	protected override string GetDoStr(bool hasCond)
	{
		string ret = "trigger:PlayStandCns(luaPlayer)";

		if (IsSetCtrl_1) {
			string pre = string.Empty;
			if (hasCond)
				pre = "\t\t\t\t\t\t\t\t";
			else
				pre = "\t\t\t\t\t\t";
			ret  += "\n\r" + pre + "trigger:CtrlSet(luaPlayer, 1)\n\r";
		}

		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/创建飞行物")]
public class AI_StateEvent_CreateProj: AI_CreateStateEvent
{
	public int projid = -1;
	public int projanim = CNSStateDef._cNoVaildAnim;
	public int projhitanim = CNSStateDef._cNoVaildAnim;
	public int projremove = 1;
	public int projcancelanim = CNSStateDef._cNoVaildAnim;
	public int projremanim = CNSStateDef._cNoVaildAnim;
	public float projremovetime = 100;
	public int offset_x = 0;
	public int offset_y = 0;
	public float velocity_x = 0;
	public float velocity_y = 0;
	public ExplodPosType Postype = ExplodPosType.p1;
	public int projshadow = 0;
	public int projpriority = 0;
	public int projsprpriority = 0;

	protected override string GetDoStr(bool hasCond)
	{
		string pre = string.Empty;
		if (hasCond)
			pre = "\t\t\t\t\t\t\t\t";
		else
			pre = "\t\t\t\t\t\t";
		
		string ret = "local proj = trigger:CreateProj(luaPlayer)\n\r";
		if (projid >= 0)
			ret += string.Format ("{0}proj.projid = {1:D}\n\r", pre, projid);

		if (VaildAnim(projanim))
			ret += string.Format ("{0}proj.projanim = {1:D}\n\r", pre, projanim);

		if (VaildAnim(projhitanim))
			ret += string.Format ("{0}proj.projhitanim = {1:D}\n\r", pre, projhitanim);

		if (VaildAnim(projremanim))
			ret += string.Format ("{0}proj.projremanim = {1:D}\n\r", pre, projremanim);

		if (VaildAnim(projcancelanim))
			ret += string.Format ("{0}proj.projcancelanim = {1:D}\n\r", pre, projcancelanim);

		if (offset_x != 0)
			ret += string.Format ("{0}proj.offset_x = {1:D}\n\r", pre, offset_x);

		if (offset_y != 0)
			ret += string.Format ("{0}proj.offset_x = {1:D}\n\r", pre, offset_y);

		if (VaildStr(velocity_x, false) != "nil")
			ret += string.Format ("{0}proj.velocity_x = {1}\n\r", pre, velocity_x.ToString());

		if (VaildStr(velocity_y, false) != "nil")
			ret += string.Format ("{0}proj.velocity_y = {1}\n\r", pre, velocity_y.ToString());

		if (Postype != ExplodPosType.p1)
			ret += string.Format ("{0}proj.Postype = {1}.{2}\n\r", pre, Postype.GetType().FullName, Postype.ToString());

		if (projshadow != 0)
			ret += string.Format ("{0}proj.projshadow = {1:D}\n\r", pre, projshadow);

		if (projpriority != 0)
			ret += string.Format ("{0}proj.projpriority = {1:D}\n\r", pre, projpriority);

		if (projsprpriority != 0)
			ret += string.Format ("{0}proj.projsprpriority = {1:D}\n\r", pre, projsprpriority);

		if (projremovetime >= 0)
			ret += string.Format ("{0}proj.projremovetime = {1}\n\r", pre, projremovetime.ToString());

		ret += string.Format ("{0}proj.projremove = {1:D}\n\r", pre, projremove);
		ret += pre + "proj:Apply()\n\r";

		return ret;
	}

}

[CreateNodeMenu("AI/创建StateEvent/删除爆炸物")]
public class AI_StateEvent_RemoveExplod: AI_CreateStateEvent
{
	public int id = -1;

	protected override string GetDoStr(bool hasCond)
	{
		if (id >= 0) {
			string ret = string.Format ("trigger:RemoveExplod(luaPlayer, {0:D})", id);
			return ret;
		}
		return string.Empty;
	}
}

/*
[CreateNodeMenu("AI/创建StateEvent/切换调色板")]
public class AI_StateEvent_ChangePallet: AI_CreateStateEvent
{
	public int index;
}
*/

[CreateNodeMenu("AI/创建StateEvent/设置float var")]
public class AI_StateEvent_floatVarSet: AI_CreateStateEvent
{
	public int index;
	public float value;

	protected override string GetDoStr(bool hasCond)
	{
		string ret = string.Format ("trigger:fVarSet(luaPlayer, {0:D}, {1})", index, value.ToString());
		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/设置int var")]
public class AI_StateEvent_intVarSet: AI_CreateStateEvent
{
	public int index;
	public int value;

	protected override string GetDoStr(bool hasCond)
	{
		string ret = string.Format ("trigger:VarSet(luaPlayer, {0:D}, {1:D})", index, value);
		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/角色暂停")]
public class AI_StateEvent_PlayerPause: AI_CreateStateEvent
{
	public float time;

	protected override string GetDoStr(bool hasCond)
	{
		string ret = string.Format ("trigger:Pause(luaPlayer, {0})", time.ToString());
		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/速度乘法")]
public class AI_StateEvent_VelMul: AI_CreateStateEvent
{
	public float velX = CNSStateDef._cNoVaildVelset;
	public float velY = CNSStateDef._cNoVaildVelset;

	protected override string GetDoStr(bool hasCond)
	{
		string velXStr = VaildStr (velX, false);
		string velYStr = VaildStr (velY, false);
		if (velXStr == "nil" && velYStr == "nil")
			return string.Empty;
		string ret = string.Format ("trigger:VelMul(luaPlayer, {0}, {1})", velXStr, velYStr);
		return ret;
	}
}

[CreateNodeMenu("AI/创建StateEvent/删除角色自己")]
public class AI_StateEvent_PlayerDestroySelf: AI_CreateStateEvent
{
	protected override string GetDoStr(bool hasCond)
	{
		return "trigger:DestroySelf(luaPlayer)";
	}
}