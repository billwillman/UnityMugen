using UnityEditor;
using UnityEngine;
using System.Collections;
using XNode;
using XNodeEditor;
using LitJson;
using Utils;

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

		menu.AddItem (new GUIContent ("导出Json"), false,
			ExportToJson
		);
	}

	private void ExportToLua()
	{
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
