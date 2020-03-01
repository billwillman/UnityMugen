using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Mugen;

[CustomEditor(typeof(DefaultLoaderPlayer))]
public class DefaultPlayerLoaderEditor : Editor {

	private void DrawCustom()
	{
		if (EditorApplication.isPlaying) {
			if (GUILayout.Button ("创建角色")) {
			
			}
		}
	}

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		DrawCustom ();
	}
}
