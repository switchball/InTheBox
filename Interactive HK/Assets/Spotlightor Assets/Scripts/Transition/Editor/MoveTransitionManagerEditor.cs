using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MoveTransitionManager))]
[CanEditMultipleObjects()]
public class MoveTransitionManagerEditor : Editor
{
	private MoveTransitionManager Target {
		get { return target as MoveTransitionManager;}
	}
	
	private Vector3 SerializedPosIn {
		get { return serializedObject.FindProperty ("posIn").vector3Value;}
		set { serializedObject.FindProperty ("posIn").vector3Value = value; }
	}
	
	private Vector3 SerializedPosOut {
		get { return serializedObject.FindProperty ("posOut").vector3Value;}
		set { serializedObject.FindProperty ("posOut").vector3Value = value; }
	}

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();
		
		GUILayoutOption setButtonLayoutOption = GUILayout.Width (50);
		GUILayoutOption moveButtonLayoutOption = GUILayout.Width (100);
		
		GUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Set As Position:", GUILayout.Width (110));
		if (GUILayout.Button ("In =", setButtonLayoutOption)) 
			SerializedPosIn = Target.transform.localPosition;
		if (GUILayout.Button ("Out =", setButtonLayoutOption)) 
			SerializedPosOut = Target.transform.localPosition;
		GUILayout.EndHorizontal ();
		
		GUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Move to Position:", GUILayout.Width (110));
		if (GUILayout.Button ("Transition In", moveButtonLayoutOption)) 
			Target.transform.localPosition = SerializedPosIn;
		if (GUILayout.Button ("Transition Out", moveButtonLayoutOption)) 
			Target.transform.localPosition = SerializedPosOut;
		GUILayout.EndHorizontal ();

		serializedObject.ApplyModifiedProperties ();
	}
}
