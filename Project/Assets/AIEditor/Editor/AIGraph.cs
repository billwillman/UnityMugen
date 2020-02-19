using UnityEditor;
using UnityEngine;
using System.Collections;
using XNode;
using XNodeEditor;

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
	}

	private void ExportToLua()
	{
	}
}
