using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MultiTransitionsManager))]
[CanEditMultipleObjects()]
public class MultiTransitionsManagerEditor : Editor
{
	private MultiTransitionsManager Target {
		get { return target as MultiTransitionsManager;}
	}

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();
		
		GUILayoutOption activeChildrenButtonLayout = GUILayout.Width (100);
		GUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Children:", GUILayout.Width (100));
		if (GUILayout.Button ("Active", activeChildrenButtonLayout)) 
			Target.childTransitions.ForEach (t => t.gameObject.SetActive (true));
		if (GUILayout.Button ("Deactive", activeChildrenButtonLayout)) 
			Target.childTransitions.ForEach (t => t.gameObject.SetActive (false));
		GUILayout.EndHorizontal ();
	}
}
