using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor (typeof(Variator), true)]
[CanEditMultipleObjects ()]
public class VariatorEditor : Editor
{
	public Variator TargetVariator{ get { return target as Variator; } }

	private bool isEditingMultipleObjects = false;

	public override void OnInspectorGUI ()
	{
		isEditingMultipleObjects = serializedObject.isEditingMultipleObjects;

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Random")) {
			foreach (Object t in targets)
				(t as Variator).RandomVariation ();
			return;
		}
		if (GUILayout.Button ("Random All")) {
			foreach (Object t in targets) {
				if (t != null)
					(t as Variator).RandomAllVariators ();
			}
			return;
		}
		GUILayout.EndHorizontal ();

//		GUILayout.BeginHorizontal ();
//		Variator.Variation[] variations = TargetVariator.Variations;
//		foreach (Variator.Variation variation in variations) {
//			if (GUILayout.Button (variation.name, GUILayout.ExpandWidth (false)))
//				variation.Apply (TargetVariator.gameObject);
//		}
//		GUILayout.EndHorizontal ();

		base.OnInspectorGUI ();
	}

	void OnSceneGUI ()
	{
		if (TargetVariator == null || isEditingMultipleObjects)
			return;

		Handles.BeginGUI ();

		int height = 23;
		int width = 600;
//		Vector2 point = HandleUtility.WorldToGUIPoint (TargetVariator.GetComponentInChildren<Renderer> ().bounds.center);
		Vector2 point = Vector2.zero;
		List<Variator> allVariators = new List<Variator> (TargetVariator.gameObject.GetComponents<Variator> ());
		int index = allVariators.IndexOf (TargetVariator);
		if (index == 0) {
			GUI.color = Color.green;
			GUILayout.BeginArea (new Rect (point, new Vector2 (width, height)));
			if (GUILayout.Button ("Random All", GUILayout.Width (120))) {
				allVariators.ForEach (v => v.RandomVariation ());
				return;
			}
			GUILayout.EndArea ();
			GUI.color = Color.white;
		}
		point.y += height * (index + 1);

		GUILayout.BeginArea (new Rect (point, new Vector2 (width, height)));
		GUILayout.BeginHorizontal ();

		GUI.color = Color.green;
		Variator.Variation[] variations = TargetVariator.Variations;
		if (variations != null && variations.Length > 1 && GUILayout.Button ("Random", GUILayout.Width (60))) {
			TargetVariator.RandomVariation ();
			return;
		}

		if (variations != null) {
			GUI.color = Color.white;
			foreach (Variator.Variation variation in variations) {
				if (GUILayout.Button (variation.name, GUILayout.ExpandWidth (false))) {
					Undo.RecordObject (TargetVariator, string.Format ("Variation {0}", variation.name));
					variation.Apply (TargetVariator.gameObject);
					if (TargetVariator != null)
						EditorUtility.SetDirty (TargetVariator);
				}
			}
		}

		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();
		Handles.EndGUI ();
	}
}
