using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor (typeof(PrefabEditorSpawner))]
[CanEditMultipleObjects ()]
public class PrefabEditorSpawnerEditor : Editor
{
//	private Object Prefab {
//		get { return this.serializedObject.FindProperty ("prefab").objectReferenceValue; }
//		set { this.serializedObject.FindProperty ("prefab").objectReferenceValue = value; }
//	}

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		foreach (Object currentTarget in targets) {
			PrefabEditorSpawner spawner = currentTarget as PrefabEditorSpawner;

			if (spawner.prefab != null) {
				int instancesCount = 0;
				for (int i = 0; i < spawner.transform.childCount; i++) {
					Transform child = spawner.transform.GetChild (i);
					if (PrefabUtility.GetPrefabParent (child.gameObject) == spawner.prefab)
						instancesCount++;
				}
				EditorGUILayout.LabelField ("Instances Count = " + instancesCount.ToString ());
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button ("+")) {
					GameObject instance = PrefabUtility.InstantiatePrefab (spawner.prefab) as GameObject;
					instance.transform.SetParent (spawner.transform, false);
				}
				GUILayout.EndHorizontal ();
			}
		}
	}
}
