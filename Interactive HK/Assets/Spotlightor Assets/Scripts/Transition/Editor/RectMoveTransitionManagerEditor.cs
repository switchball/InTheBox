using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(RectMoveTransitionManager))]
[CanEditMultipleObjects()]
public class RectMoveTransitionManagerEditor : Editor {

	private RectMoveTransitionManager Target {
		get { return target as RectMoveTransitionManager;}
	}
	
	private Vector3 SerializedPosIn {
		get { return serializedObject.FindProperty ("anchorPosIn").vector2Value;}
		set { serializedObject.FindProperty ("anchorPosIn").vector2Value = value; }
	}
	
	private Vector3 SerializedPosOut {
		get { return serializedObject.FindProperty ("anchorPosOut").vector2Value;}
		set { serializedObject.FindProperty ("anchorPosOut").vector2Value = value; }
	}
	
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();
		
		GUILayoutOption setButtonLayoutOption = GUILayout.Width (50);
		GUILayoutOption moveButtonLayoutOption = GUILayout.Width (90);

		GUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Set As Position:", GUILayout.Width (110));
		if (GUILayout.Button ("In =", setButtonLayoutOption)) 
			SerializedPosIn = Target.GetComponent<RectTransform>().anchoredPosition;
		if (GUILayout.Button ("Out =", setButtonLayoutOption)) 
			SerializedPosOut = Target.GetComponent<RectTransform>().anchoredPosition;
		GUILayout.EndHorizontal ();
		
		GUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Move to Position:", GUILayout.Width (110));
		if (GUILayout.Button ("Transition In", moveButtonLayoutOption)) 
			Target.GetComponent<RectTransform>().anchoredPosition = SerializedPosIn;
		if (GUILayout.Button ("Transition Out", moveButtonLayoutOption)) 
			Target.GetComponent<RectTransform>().anchoredPosition = SerializedPosOut;
		GUILayout.EndHorizontal ();
		
		serializedObject.ApplyModifiedProperties ();
	}
}
