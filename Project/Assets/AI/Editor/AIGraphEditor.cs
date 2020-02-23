using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using XNodeEditor;
using LitJson;
using Utils;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace XNode.Mugen
{
	[CustomNodeGraphEditor(typeof(AIGraph))]
	public class AINodeGraphEditor: NodeGraphEditor
	{
		public override void AddContextMenuItems(GenericMenu menu)
		{
			base.AddContextMenuItems (menu);
			menu.AddItem (new GUIContent ("导出Lua"), false,
				ExportToLua
			);

			//	menu.AddItem (new GUIContent ("导出Tree"), false,
			//		ExportToTree
			//	);

			//	menu.AddItem (new GUIContent ("加载Tree"), false,
			//		LoadFromTree
			//	);
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

		private void BuildStr(AI_CreateStateDef def, StringBuilder builder)
		{
			if (def == null || builder == null)
				return;
			builder.AppendFormat ("--------------------------- register StateDef {0} ---------------------------", def.Animate).AppendLine ();
			builder.Append (def.ToString ()).AppendLine ().AppendLine ();
		}

		private void BuildStr(List<AI_CreateStateDef> defs, StringBuilder builder)
		{
			if (defs == null || defs.Count <= 0 || builder == null)
				return;
			for (int i = 0; i < defs.Count; ++i) {
				var def = defs [i];
				if (def == null)
					continue;
				BuildStr (def, builder);
			}
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
				builder.Append (preStr).Append ("return triggle1").AppendLine ();
			} else {
				builder.Append (preStr).Append ("return true").AppendLine ();
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
			builder.Append ("\t\t").Append ("\t\t").Append("end").AppendLine ().AppendLine ();
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

			string path = AssetDatabase.GetAssetPath (target);
			path = System.IO.Path.GetDirectoryName (path).Replace ('\\', '/');
			string name = System.IO.Path.GetFileNameWithoutExtension (path);
			builder.AppendFormat ("function {0}:initCmd_{1}(luaCfg)\n\r", name, target.name);

			var keyCmdLst = GetKeyCmds<AI_KeyCmd> (lst);
			// 按键
			BuildStr (keyCmdLst, builder);
			var aiCmdLst = GetKeyCmds<AI_Cmd> (lst);
			BuildStr (aiCmdLst, builder);

			var stateDefs = GetKeyCmds<AI_CreateStateDef> (lst);
			BuildStr (stateDefs, builder);

			builder.Append ("end\n\r");

			SaveToLuaFile (builder);
		}

		private void SaveToLuaFile(StringBuilder builder)
		{
			var target = this.target;
			if (target == null)
				return;

			if (builder == null)
				return;

			string path = AssetDatabase.GetAssetPath (target);
			path = System.IO.Path.GetDirectoryName (path).Replace ('\\', '/');

			string fileName = string.Format ("{0}/{1}.txt", path, target.name);
			System.IO.FileStream stream = new System.IO.FileStream (fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
			try
			{
				string ss = builder.ToString();
				if (!string.IsNullOrEmpty(ss))
				{
					byte[] buf = System.Text.Encoding.UTF8.GetBytes(ss);
					stream.Write(buf, 0, buf.Length);
				}
			} finally {
				stream.Close ();
				stream.Dispose ();
			}

			AssetDatabase.Refresh ();
		}

		private void LoadFromTree()
		{
			var target = this.target;
			if (target == null)
				return;
			string path = AssetDatabase.GetAssetPath (target);
			path = System.IO.Path.GetDirectoryName (path).Replace ('\\', '/');

			string fileName = string.Format ("{0}/{1}.bin", path, target.name);
			if (System.IO.Directory.Exists (fileName)) {
				IFormatter formatter = new BinaryFormatter ();
				System.IO.FileStream stream = new System.IO.FileStream (fileName, System.IO.FileMode.Open, System.IO.FileAccess.Write);
				try
				{
					var nodes = formatter.Deserialize(stream) as List<Node>;
					if (nodes == null)
					{
						Debug.LogErrorFormat ("[LoadFromTree] DeSerialeze Failed: {0}", fileName);
					} else
					{
						if (this.window != null)
						{
							this.window.Close();
						}
						this.serializedObject = null;
						target.nodes = nodes;
						NodeEditorWindow.OnOpen(target.GetInstanceID(), 0);
					}
				} finally {
					stream.Close ();
					stream.Dispose ();
				}
			} else
				Debug.LogErrorFormat ("[LoadFromTree] not found file: {0}", fileName);
		}

		private void ExportToTree()
		{
			var target = this.target;
			if (target == null)
				return;

			string path = AssetDatabase.GetAssetPath (target);
			path = System.IO.Path.GetDirectoryName (path).Replace ('\\', '/');

			string fileName = string.Format ("{0}/{1}.bin", path, target.name);
			IFormatter formatter = new BinaryFormatter();
			System.IO.FileStream stream = new System.IO.FileStream (fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
			try
			{
				formatter.Serialize(stream, target.nodes);
			} finally {
				stream.Close ();
				stream.Dispose ();
			}

			AssetDatabase.Refresh ();
		}


	}

}