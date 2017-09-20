using UnityEngine;
using UnityEditor;
using System.Collections;

public class TransformRandomizer : ScriptableWizard
{
	public float rotateAngle = 0;
	public Vector3 rotateAxis = Vector3.up;
	public Vector3 offset = Vector3.zero;

	[MenuItem("GameObject/Random Transform")]
	static void DisplayWizard ()
	{
		ScriptableWizard.DisplayWizard<TransformRandomizer> ("Random Transform", "Randomizer");
	}
	
	private void OnWizardCreate ()
	{
		Object[] objects = Selection.GetFiltered (typeof(Transform), SelectionMode.TopLevel);
		
		Undo.RecordObjects (objects, "Random Transform");
		for (int i = 0; i < objects.Length; i++) {
			Transform transform = objects [i] as Transform;
			
			if (rotateAngle != 0)
				transform.Rotate (rotateAxis, Random.Range (0, rotateAngle), Space.Self);

			if (offset != Vector3.zero)
				transform.position += new Vector3 (Random.Range (0, offset.x), Random.Range (0, offset.y), Random.Range (0, offset.z));
		}
	}
}
