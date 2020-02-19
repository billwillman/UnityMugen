using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;

[CreateNodeMenu("AI/按键设置")]
public class AI_KeyCmd : Node {
	public string name = "KeyCmd_Unknown";
	public float time = 1;
	public string keyCommands;

	[Output(ShowBackingValue.Never)]
	public AI_KeyCmd output;
	public override object GetValue(NodePort port) {
		return output;
	}
}


[CreateNodeMenu("AI/AI命令")]
public class AI_Cmd : Node
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
}

public abstract class AI_BaseCondition : Node
{
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
}

[CreateNodeMenu("AI/条件/Or")]
public class AI_Cond_Or: AI_BaseCondition
{
	[Input(ShowBackingValue.Never)]
	public List<AI_BaseCondition> inputs;
	public override object GetValue(NodePort port) {
		return inputs;
	}
}

[CreateNodeMenu("AI/条件/触发按键")]
public class AI_Cond_TriggleKeyCmd: AI_BaseCondition
{
	[Input(ShowBackingValue.Never)]
	public AI_KeyCmd aiKeyCmd;
	public override object GetValue(NodePort port) {
		if (port.fieldName == "aiKeyCmd")
			return aiKeyCmd;
		return null;
	}

	public bool isNot = false;
}

[CreateNodeMenu("AI/条件/角色StateType状态")]
public class AI_Cond_PlayerStateType: AI_BaseCondition
{
	public Cns_Type stateType = Cns_Type.S;

	public bool isNot = false;	
}

[CreateNodeMenu("AI/条件/角色Ctrl状态")]
public class AI_Cond_PlayerCtrl: AI_BaseCondition
{
	public int Ctrl = 1;
	public bool isNot = false;	
}

public enum AI_Cond_Op
{
	Less,
	LessOrEqual,
	Great,
	GreatAndEqual,
	Equal
}

[CreateNodeMenu("AI/条件/角色AnimElem")]
public class AI_Cond_PlayerAniElem: AI_BaseCondition
{
	public int aniElem;
	public AI_Cond_Op op = AI_Cond_Op.Equal;
}

[CreateNodeMenu("AI/条件/角色AnimTime")]
public class AI_Cond_PlayerAniTime: AI_BaseCondition
{
	public int aniTime;
	public AI_Cond_Op op = AI_Cond_Op.Equal;
}

[CreateNodeMenu("AI/创建StateDef")]
public class AI_CreateStateDef: Node
{
	[Input(ShowBackingValue.Never)]
	public AI_Cmd input;
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
		return null;
	}

	public override void OnCreateConnection(NodePort from, NodePort to) { 
		if (from != null && from.ValueType == typeof(AI_Cmd)) {
			AI_Cmd aiCmd = (AI_Cmd)from.node;
			if (aiCmd.aiType != AI_Type.ChangeState) {
				var port = GetInputPort("input");
				if (port != null)
					port.ClearConnections ();
			}
		}
	}
}

//[CreateNodeMenu("AI/创建StateEvent")]
public abstract class AI_CreateStateEvent: Node
{
	[Input(ShowBackingValue.Never)]
	public AI_CreateStateDef parent;

	[Input(ShowBackingValue.Never)]
	public AI_BaseCondition conditions;

	public CnsStateTriggerType triggleType = CnsStateTriggerType.AnimElem;

	public override object GetValue(NodePort port)
	{
		if (port.fieldName == "parent")
			return parent;
		else if (port.fieldName == "conditions")
			return conditions;
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

