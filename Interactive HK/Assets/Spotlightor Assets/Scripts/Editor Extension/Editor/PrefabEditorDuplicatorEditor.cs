using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(PrefabEditorDuplicator), true)]
public class PrefabEditorDuplicatorEditor : Editor
{
	private PrefabEditorDuplicator Duplicator{ get { return target as PrefabEditorDuplicator; } }

	void OnSceneGUI ()
	{
		if (!Duplicator.enabled)
			return;

		GameObject targetPrefab = PrefabUtility.GetPrefabParent (Duplicator.gameObject) as GameObject;
		if (targetPrefab != null) {
			if (Duplicator.duplicateWaypoints != null) {
				foreach (PrefabEditorDuplicator.DuplicateWaypoint waypoint in Duplicator.duplicateWaypoints)
					DrawInsertNewPartButton (targetPrefab, waypoint);
			}
		}
	}

	private bool DrawInsertNewPartButton (GameObject prefab, PrefabEditorDuplicator.DuplicateWaypoint waypoint)
	{
		if (prefab != null)
			Handles.color = new Color (0, 1, 0, 0.5f);

		Vector3 buttonPos = Duplicator.transform.TransformPoint (waypoint.position);
		Quaternion buttonRot = Duplicator.transform.rotation * Quaternion.Euler (waypoint.rotation);
		buttonPos += buttonRot * Duplicator.buttonOffset;

		if (Handles.Button (buttonPos, buttonRot, Duplicator.buttonSize, Duplicator.buttonSize, Handles.CubeCap)) {
			InstantiatePrefabAt (prefab, waypoint);
			return true;
		} else
			return false;
	}

	private GameObject InstantiatePrefabAt (GameObject prefab, PrefabEditorDuplicator.DuplicateWaypoint waypoint)
	{
		GameObject instance = PrefabUtility.InstantiatePrefab (prefab) as GameObject;

		instance.transform.SetParent (Duplicator.transform.parent, false);
		instance.transform.SetSiblingIndex (Duplicator.transform.GetSiblingIndex () + 1);
		instance.transform.rotation = Duplicator.transform.rotation * Quaternion.Euler (waypoint.rotation);
		instance.transform.position = Duplicator.transform.TransformPoint (waypoint.position);

		Selection.activeGameObject = instance;

		#if UNITY_5
		UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty (UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene ());
		#else
		EditorApplication.MarkSceneDirty ();
		#endif

		return instance;
	}
}
