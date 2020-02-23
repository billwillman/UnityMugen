using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{

[CreateNodeMenu("AI/AI命令")]
[Serializable]
public class AI_Cmd : AI_BaseNode
{
	
	[SerializeField] public string cmdName = "AICmd_Unknown";

	[SerializeField] public AI_Type aiType = AI_Type.ChangeState;

	[SerializeField] public string value;

	[Input(ShowBackingValue.Never)]
	[SerializeField] public List<AI_BaseCondition> condList;

	[Output(ShowBackingValue.Never)]
	[SerializeField] public AI_Cmd output;

	public override object GetValue(NodePort port) {
		if (port.fieldName == "condList")
			return condList;
		else if (port.fieldName == "output")
			return output;
		return null;
	}

	public override void OnRemoveConnection(NodePort port)
	{
			base.OnRemoveConnection(port);
		DoDisConnectToList<AI_BaseCondition> (port, ref condList);
	}

	public override void OnCreateConnection(NodePort from, NodePort to)
	{
			base.OnCreateConnection (from, to);
		DoCreateConnectToList<AI_BaseCondition> (from, to, ref condList, "condList");
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

/*
[CreateNodeMenu("AI/创建StateEvent/切换调色板")]
public class AI_StateEvent_ChangePallet: AI_CreateStateEvent
{
	public int index;
}
*/


}