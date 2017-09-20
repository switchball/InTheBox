#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
public class PrefabInstancesCategorizer : MonoBehaviour
{
#if !UNITY_EDITOR
	void Awake ()
	{
		Destroy (this);
	}
#endif

#if UNITY_EDITOR
	void Update ()
	{
		if (!Application.isPlaying)
			CategorizeChildren (transform);
	}

	private void CategorizeChildren (Transform parent)
	{
		for (int i = 0; i < parent.childCount; i++) {
			Transform child = parent.GetChild (i);
			Object prefab = PrefabUtility.GetPrefabParent (child.gameObject);
			if (prefab != null) {
				string prefabPath = AssetDatabase.GetAssetPath (prefab);
				int lastSlashIndex = prefabPath.LastIndexOf ("/");
				int lastLastSlashIndex = prefabPath.LastIndexOf ("/", lastSlashIndex - 1);
				string folderName = prefabPath.Substring (lastLastSlashIndex + 1, lastSlashIndex - lastLastSlashIndex - 1);
				
				Transform root = transform.Find (folderName);
				if (root == null) {
					GameObject rootGo = new GameObject (folderName);
					root = rootGo.transform;
					root.SetParent (transform, false);
					
					this.Log ("Category root gameObject {0} created", rootGo.name);
				}

				bool inRoot = false;
				Transform childParent = child.parent;
				while (childParent != null && !inRoot) {
					inRoot = childParent == root;
					childParent = childParent.parent;
				}
				
				if (!inRoot) {
					child.SetParent (root, true);
					
					this.Log ("{0} categorized to {1}", child.name, root.name);
					i--;
				}
			} else
				CategorizeChildren (child);
		}
	}
#endif
}
