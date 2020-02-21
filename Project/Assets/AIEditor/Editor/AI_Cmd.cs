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

	protected void DoCreateConnect<T>(NodePort from, ref T item, string itemName, NodePort.IO dir = NodePort.IO.Input) where T: Node
	{
		if (dir == NodePort.IO.Input) {
			if (from.node == this)
				return;
		} else if (dir == NodePort.IO.Output) {
			if (from.node != this)
				return;
		}
		//if (from.node.GetType ().IsSubclassOf (typeof(T))) {
			item =  from.node as T;
		//}
		if (item == null) {
			var port = GetInputPort (itemName);
			if (port != null) {
				port.Disconnect (from);
			}
		}
	}

	protected void DoCreateConnectToList<T>(NodePort from, ref List<T> condList, string condListName, NodePort.IO dir = NodePort.IO.Input) where T: Node
	{
		if (dir == NodePort.IO.Input) {
			if (from.node == this)
				return;
		} else if (dir == NodePort.IO.Output) {
			if (from.node != this)
				return;
		}

		if (from.node is T) {
			if (condList == null)
				condList = new List<T> ();
			T cc = from.node as T;	
			if (!condList.Contains (cc)) {
				condList.Add (cc);
			}
		} else {
			var port = GetInputPort (condListName);
			int idx = port.GetConnectionIndex (from);
			if (idx >= 0)
				port.Disconnect (idx);
		}
	}

	protected void DoDisConnect<T>(NodePort port, ref T item, NodePort.IO dir = NodePort.IO.Input) where T: Node
	{
		if (port.direction != dir)
			return;
		
		for (int i = 0; i < port.ConnectionCount; ++i) {
			T cc = port.GetConnection(i).node as T;
			if (cc != null) {
				item = cc;
				return;
			}
		}
		item = null;
	}

	protected void DoDisConnectToList<T>(NodePort port, ref List<T> condList, NodePort.IO dir = NodePort.IO.Input) where T: Node
	{
		if (port.direction != dir)
			return;
		
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



[CreateNodeMenu("AI/创建StateDef")]
public class AI_CreateStateDef: AI_BaseNode
{

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

		ret = string.Format ("\t\tlocal id = luaCfg:CreateStateDef(\"{0}\")\n\r", input.value);
		ret += "\t\tlocal def = luaCfg:GetStateDef(id)\n\r";
		if (type != Cns_Type.none)
			ret += string.Format ("\t\tdef.Type = {0}.{1}\n\r", type.GetType ().FullName, type.ToString ());
		if (physicsType != Cns_PhysicsType.none)
			ret += string.Format ("\t\tdef.PhysicsType = {0}.{1}\n\r", physicsType.GetType ().FullName, physicsType.ToString ());
		if (moveType != Cns_MoveType.none)
			ret += string.Format ("\t\tdef.MoveType = {0}.{1}\n\r", moveType.GetType ().FullName, moveType.ToString ());
		ret += string.Format ("\t\tdef.Juggle = {0:D}\n\r", juggle);
		ret += string.Format ("\t\tdef.PowerAdd = {0:D}\n\r", juggle);

		if (Mathf.Abs (velset_x - CNSStateDef._cNoVaildVelset) > float.Epsilon) {
			ret += string.Format ("\t\tdef.Velset_x = {0}\n\r", velset_x.ToString ());
		}

		if (Mathf.Abs (velset_y - CNSStateDef._cNoVaildVelset) > float.Epsilon) {
			ret += string.Format ("\t\tdef.Velset_y = {0}\n\r", velset_y.ToString ());
		}

		ret += string.Format ("\t\tdef.Ctrl = {0:D}\n\r", ctrl);
		ret += string.Format ("\t\tdef.Sprpriority = {0:D}\n\r", sprpriority);
		if (!string.IsNullOrEmpty(animate))
			ret += string.Format ("\t\tdef.Animate = {0}\n\r", animate);

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
		
		if (from != null && from.node.GetType () == typeof(AI_Cmd)) {
			DoCreateConnect<AI_Cmd> (from, ref input, "input"); 
		} else if (from != null && from.node.GetType () == typeof(AI_StateEvent_PlayCns)) {
			DoCreateConnect<AI_StateEvent_PlayCns> (from, ref prevCns, "prevCns");
		} else {
			from.Disconnect (to);
		}
	}

	public override void OnRemoveConnection(NodePort port)
	{
		if (port.direction != NodePort.IO.Input)
			return;

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
	[Input(ShowBackingValue.Never)]
	public AI_CreateStateDef parent;

	[Input(ShowBackingValue.Never)]
	public AI_BaseCondition condition;

	public bool setPersistent = false;

	public CnsStateTriggerType triggleType = CnsStateTriggerType.AnimElem;

	public override void OnCreateConnection(NodePort from, NodePort to)
	{
		if (from != null && (from.node.GetType() == typeof(AI_Cond_PlayerTime) ||
			from.node.GetType() == typeof(AI_Cond_PlayerAniTime))) {
			triggleType = CnsStateTriggerType.AnimTime;
		} else if ((from != null) && (from.node.GetType() == typeof(AI_Cond_PlayerAniElem)) ||
			(from.node.GetType() == typeof(AI_Cond_PlayerAnimElemTime))) {
			triggleType = CnsStateTriggerType.AnimElem;
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
}

[CreateNodeMenu("AI/创建StateEvent/播放声音")]
public class AI_StateEvent_PlaySnd: AI_CreateStateEvent
{
	public int group;
	public int index;
}

[CreateNodeMenu("AI/创建StateEvent/设置速度")]
public class AI_StateEvent_VelSet: AI_CreateStateEvent
{
	public float velx = CNSStateDef._cNoVaildVelset;
	public float vely = CNSStateDef._cNoVaildVelset;
}

[CreateNodeMenu("AI/创建StateEvent/速度增加")]
public class AI_StateEvent_VelAdd: AI_CreateStateEvent
{
	public float velx = CNSStateDef._cNoVaildVelset;
	public float vely = CNSStateDef._cNoVaildVelset;
}

[CreateNodeMenu("AI/创建StateEvent/位置增加")]
public class AI_StateEvent_PosAdd: AI_CreateStateEvent
{
	public int x = CNSStateDef._cNoVaildVelset;
	public int y = CNSStateDef._cNoVaildVelset;
}

[CreateNodeMenu("AI/创建StateEvent/设置位置")]
public class AI_StateEvent_PosSet: AI_CreateStateEvent
{
	public int x = CNSStateDef._cNoVaildVelset;
	public int y = CNSStateDef._cNoVaildVelset;
}

[CreateNodeMenu("AI/创建StateEvent/StateType设置")]
public class AI_StateEvent_StateTypeSet: AI_CreateStateEvent
{
	public Cns_Type stateType = Cns_Type.S;
}

[CreateNodeMenu("AI/创建StateEvent/Ctrl设置")]
public class AI_StateEvent_CtrlSet: AI_CreateStateEvent
{
	public int ctrl = 1;
}

[CreateNodeMenu("AI/创建StateEvent/创建爆炸")]
public class AI_StateEvent_CreateExplod: AI_CreateStateEvent
{
	public int anim = CNSStateDef._cNoVaildAnim;
	public int id = 0;
	public int pos_x = 0;
	public int pos_y = 0;
	public ExplodPosType explodPosType = ExplodPosType.p1;
	public int bindtime = 0;
	public int removetime = -2;
	public int sprpriority = 0;
	public int removeongethit = 0;
	public int ignorehitpause = 1;
	public bool isChangeStateRemove = true;
	public bool IsUseParentUpdate = true;
}

[CreateNodeMenu("AI/创建StateEvent/播放动画")]
public class AI_StateEvent_PlayAni: AI_CreateStateEvent
{
	public int anim = CNSStateDef._cNoVaildAnim;
	public bool isLoop = false;
	public bool isCleanStateDef = true;
}

[CreateNodeMenu("AI/创建StateEvent/切换Cns状态")]
public class AI_StateEvent_PlayCns: AI_CreateStateEvent
{
	[Output(ShowBackingValue.Never)]
	public AI_CreateStateDef nextStateDef;

	public string Animate;

	public override object GetValue(NodePort port)
	{
		if (port.fieldName == "nextStateDef")
			return nextStateDef;
		return null;
	}
}

[CreateNodeMenu("AI/创建StateEvent/切换Stand状态")]
public class AI_StateEvent_PlayStandCns: AI_CreateStateEvent
{}

[CreateNodeMenu("AI/创建StateEvent/创建飞行物")]
public class AI_StateEvent_CreateProj: AI_CreateStateEvent
{
	public int projid = -1;
	public int projanim = CNSStateDef._cNoVaildAnim;
	public int projhitanim = CNSStateDef._cNoVaildAnim;
	public int projremove = 1;
	public int projcancelanim = CNSStateDef._cNoVaildAnim;
	public int projremanim = CNSStateDef._cNoVaildAnim;
	public float projremovetime = -1;
	public int offset_x = 0;
	public int offset_y = 0;
	public float velocity_x = 0;
	public float velocity_y = 0;
	public ExplodPosType Postype = ExplodPosType.p1;
	public int projshadow = 0;
	public int projpriority = 0;
	public int projsprpriority = 0;

}

[CreateNodeMenu("AI/创建StateEvent/删除爆炸物")]
public class AI_StateEvent_RemoveExplod: AI_CreateStateEvent
{
	public int id;
}

[CreateNodeMenu("AI/创建StateEvent/切换调色板")]
public class AI_StateEvent_ChangePallet: AI_CreateStateEvent
{
	public int index;
}

[CreateNodeMenu("AI/创建StateEvent/设置float var")]
public class AI_StateEvent_floatVarSet: AI_CreateStateEvent
{
	public int index;
	public float value;
}

[CreateNodeMenu("AI/创建StateEvent/设置int var")]
public class AI_StateEvent_intVarSet: AI_CreateStateEvent
{
	public int index;
	public int value;
}

[CreateNodeMenu("AI/创建StateEvent/角色暂停")]
public class AI_StateEvent_PlayerPause: AI_CreateStateEvent
{
	public float time;
}

[CreateNodeMenu("AI/创建StateEvent/速度乘法")]
public class AI_StateEvent_VelMul: AI_CreateStateEvent
{
	public float velX = CNSStateDef._cNoVaildVelset;
	public float velY = CNSStateDef._cNoVaildVelset;
}

[CreateNodeMenu("AI/创建StateEvent/删除角色自己")]
public class AI_StateEvent_PlayerDestroySelf: AI_CreateStateEvent
{}