using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TubePart), true)]
public class TubePartEditor : Editor
{
	private float length = 2;

	private TubePart Part{ get { return target as TubePart; } }

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		GUILayout.BeginHorizontal ();

		length = EditorGUILayout.FloatField ("Length", length);
		if (GUILayout.Button ("Auto Set End Position", GUILayout.Width (200))) {
			float bendAngle = 0;
			Vector3 axis = Vector3.zero;
			Quaternion.Euler (Part.endRotation).ToAngleAxis (out bendAngle, out axis);

			float bendCircleRadius = Mathf.Abs (length / (Mathf.Deg2Rad * bendAngle));
			float halfBendRad = 0.5f * bendAngle * Mathf.Deg2Rad;
			float disanceToEnd = Mathf.Abs (Mathf.Sin (halfBendRad) * bendCircleRadius * 2);
			Part.endPosition = Quaternion.AngleAxis (90f - (180f - bendAngle) * 0.5f, axis) * Vector3.forward * disanceToEnd;
		}

		GUILayout.EndHorizontal ();

		serializedObject.ApplyModifiedProperties ();
	}

	void OnSceneGUI ()
	{
		TubePlacer placer = Part.transform.GetComponentInParent<TubePlacer> ();
		if (placer != null && placer.enabled) 
			TubePlacerEditor.DrawTubePartSceneEditor (Part, placer);
	}
}
