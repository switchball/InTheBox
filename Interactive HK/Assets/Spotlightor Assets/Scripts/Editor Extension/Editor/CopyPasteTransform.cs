using UnityEditor;
using UnityEngine;
using System.Collections;

class CopyPasteTransform : ScriptableObject
{
	private static Vector3 copiedPosition = Vector3.zero;
	private static Quaternion copiedRotation = Quaternion.identity;
	private static Vector3 copiedLocalPosition = Vector3.zero;
	private static Quaternion copiedLocalRotation = Quaternion.identity;

	[MenuItem("GameObject/Copy Transform %#c", false, 200)]
	static void CopyTransform ()
	{
		if (Selection.activeGameObject != null) {
			copiedPosition = Selection.activeGameObject.transform.position;
			copiedRotation = Selection.activeGameObject.transform.rotation;
			Debug.Log (string.Format ("Position and Rotation of {0} copied.", Selection.activeObject.name));
		}
	}
	
	[MenuItem("GameObject/Paste Transform %#v", false, 201)]
	static void PasteTransform ()
	{
		GameObject[] gos = Selection.gameObjects;
		
		Undo.RecordObjects (gos, "Paste transform");
		foreach (GameObject go in gos) {
			go.transform.position = copiedPosition;
			go.transform.rotation = copiedRotation;
			Debug.Log (string.Format ("Position and Rotation of {0} set by copied transform.", go.name));
		}
	}
	
	[MenuItem("GameObject/Copy Local Transform %#&c", false, 202)]
	static void CopyLocalTransform ()
	{
		if (Selection.activeGameObject != null) {
			copiedLocalPosition = Selection.activeGameObject.transform.localPosition;
			copiedLocalRotation = Selection.activeGameObject.transform.localRotation;
			Debug.Log (string.Format ("Local Position and Rotation of {0} copied.", Selection.activeObject.name));
		}
	}
	
	[MenuItem("GameObject/Paste Local Transform %#&v", false, 203)]
	static void PasteLocalTransform ()
	{
		GameObject[] gos = Selection.gameObjects;
		
		Undo.RecordObjects (gos, "Paste local transform");
		foreach (GameObject go in gos) {
			go.transform.localPosition = copiedLocalPosition;
			go.transform.localRotation = copiedLocalRotation;
			Debug.Log (string.Format ("Local Position and Rotation of {0} set by copied transform.", go.name));
		}
	}
}