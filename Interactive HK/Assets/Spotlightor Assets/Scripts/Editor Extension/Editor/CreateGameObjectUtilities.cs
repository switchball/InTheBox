using UnityEditor;
using UnityEngine;
using System.Collections;

class CreateGameObjectUtilities : ScriptableObject
{
	[MenuItem("GameObject/Create Empty At Selected %#&n")]
	static void CreateAtSamePlace ()
	{
		GameObject[] objs = Selection.gameObjects;
		GameObject go = new GameObject ();
		if (objs.Length > 0) {
			GameObject target = objs [0];
            
			if (target.GetComponent<RectTransform> () == null) {
				go.transform.position = target.transform.position;
				go.transform.rotation = target.transform.rotation;
				go.transform.parent = target.transform.parent;
			} else {
				RectTransform rectTransform = go.AddComponent<RectTransform> ();
				rectTransform.SetParent (target.transform.parent, false);
				rectTransform.anchoredPosition = target.GetComponent<RectTransform> ().anchoredPosition;
				rectTransform.sizeDelta = Vector2.zero;
			}
			go.layer = target.layer;
		}
		
		Selection.activeObject = go;
	}
}