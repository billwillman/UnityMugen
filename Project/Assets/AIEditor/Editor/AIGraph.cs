using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XNode;
using XNodeEditor;
using LitJson;
using Utils;
using System.Text;

[CreateAssetMenu]
public class AIGraph : NodeGraph {

}

[CustomNodeGraphEditor(typeof(AIGraph))]
public class AINodeGraphEditor: NodeGraphEditor
{
	public override void AddContextMenuItems(GenericMenu menu)
	{
		base.AddContextMenuItems (menu);
		menu.AddItem (new GUIContent ("导出Lua"), false,
			ExportToLua
		);

		//menu.AddItem (new GUIContent ("导出Json"), false,
		//	ExportToJson
		//);
	}



	private List<T> GetKeyCmds<T>(List<Node> lst) where T: Node
	{
		if (lst == null || lst.Count <= 0)
			return null;

		List<T> ret = null;
		for (int i = 0; i < lst.Count; ++i) {
			var n = lst [i];
			if (n is T) {
				if (ret == null)
					ret = new List<T> ();
				ret.Add (n as T);
			}
		}
		return ret;
	}

	private void BuildStr(List<AI_KeyCmd> keyCmds, StringBuilder builder)
	{
		if (keyCmds == null || keyCmds.Count <= 0 || builder == null)
			return;
		builder.Append ("--------------------------- register KeyCmd ---------------------------").AppendLine ();
		bool isFirst = true;
		for (int i = 0; i < keyCmds.Count; ++i) {
			var cmd = keyCmds [i];
			if (cmd == null)
				continue;
			builder.Append ("\t\t");
			if (isFirst) {
				builder.Append ("local ");
				isFirst = false;
			}
			builder.AppendFormat ("cmd = luaCfg:CreateCmd(\"{0}\")", cmd.name).AppendLine();
			builder.Append ("\t\t").AppendFormat ("cmd.time = {0}", cmd.time.ToString ()).AppendLine ();
			builder.Append ("\t\t").AppendFormat ("cmd:AttachKeyCommands(\"{0}\")", cmd.keyCommands).AppendLine ().AppendLine ();
		}
	}

	private void BuildCondListStr(string preStr, string luaPlayer, List<AI_BaseCondition> lst, StringBuilder builder)
	{
		bool isFirst = true;
		bool isWrited = false;
		for (int i = 0; i < lst.Count; ++i) {
			var cond = lst [i];
			if (cond == null)
				continue;

			string str = cond.ToCondString (luaPlayer);
			if (!string.IsNullOrEmpty (str)) {
				if (isFirst) {
					builder.Append (preStr).Append ("local triggle1 = (").Append (str).Append (")").AppendLine ();
					isWrited = true;
					isFirst = false;
				} else {
					builder.Append (preStr).Append ("\t\t").Append ("and (").Append (str).Append (")").AppendLine ();
					isWrited = true;
				}
			}
		}
		if (isWrited) {
			builder.Append (preStr).Append ("return triggle1");
		}
	}

	private void BuildStr(AI_Cmd cmd, StringBuilder builder)
	{
		if (cmd == null || builder == null)
			return;
		builder.AppendFormat ("--------------------------- {0} ---------------------------", cmd.cmdName).AppendLine();
		builder.Append ("\t\t").AppendFormat ("local aiCmd = luaCfg:CreateAICmd(\"{0}\")", cmd.cmdName).AppendLine ();
		builder.Append ("\t\t").AppendFormat ("aiCmd.type = {0}.{1}", cmd.aiType.GetType ().FullName, cmd.aiType.ToString()).AppendLine ();
		builder.Append ("\t\t").AppendFormat ("aiCmd.value = \"{0}\"", cmd.value).AppendLine ();
		builder.Append ("\t\t").Append ("aiCmd.OnTriggerEvent =").AppendLine ();
		builder.Append ("\t\t").Append ("\t\t").Append ("function (luaPlayer, aiName)").AppendLine ();
		BuildCondListStr ("\t\t\t\t\t\t", "luaPlayer", cmd.condList, builder);
	}

	private void BuildStr(List<AI_Cmd> cmds, StringBuilder builder)
	{
		if (cmds == null || builder == null)
			return;
		for (int i = 0; i < cmds.Count; ++i) {
			var cmd = cmds [i];
			if (cmd == null)
				continue;
			BuildStr (cmd, builder);
		}
	}

	private void ExportToLua()
	{
		var target = this.target;
		if (target == null)
			return;
		var lst = target.nodes;
		if (lst == null || lst.Count <= 0)
			return;
		StringBuilder builder = new StringBuilder ();
		var keyCmdLst = GetKeyCmds<AI_KeyCmd> (lst);
		// 按键
		BuildStr (keyCmdLst, builder);
		var aiCmdLst = GetKeyCmds<AI_Cmd> (lst);
		BuildStr (aiCmdLst, builder);

		SaveToLuaFile (builder);
	}

	private void SaveToLuaFile(StringBuilder builder)
	{
		var target = this.target;
		if (target == null)
			return;
		
		if (builder == null)
			return;

		string fileName = string.Format ("Assets/AIEditor/{0}.lua", target.name);
		System.IO.FileStream stream = new System.IO.FileStream (fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
		try
		{
			FilePathMgr.Instance.WriteString(stream, builder.ToString());
		} finally {
			stream.Close ();
			stream.Dispose ();
		}
	}

	private void ExportToJson()
	{
		/*
		var target = this.target;
		if (target == null)
			return;
		var lst = target.nodes;
		if (lst != null) {
			
			string name = target.name;
			if (!string.IsNullOrEmpty (name)) {
				string str = LitJson.JsonMapper.ToJson (lst);
				string fileName = string.Format ("Assets/AIEditor/{0}.json", name);
				System.IO.FileStream stream = new System.IO.FileStream (fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
				try
				{
					FilePathMgr.GetInstance().WriteString(stream, str);
					Debug.LogFormat("[ExportToJson] {0} Saved~!", fileName);
				} finally {
					stream.Close ();
					stream.Dispose ();
				}
			} else {
				Debug.LogError ("[ExportToJson] name is null");
			}

		}
		*/
	}
}
