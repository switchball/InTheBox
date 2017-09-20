using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
public class ChildSiblingIndexByName : MonoBehaviour
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
		if (Application.isEditor && !Application.isPlaying)
			UpdateChildrenSiblingIndexes ();
	}
	
	private void UpdateChildrenSiblingIndexes ()
	{
		List<Transform> children = new List<Transform> ();
		for (int i = 0; i < transform.childCount; i++)
			children.Add (transform.GetChild (i));

		children.Sort (
		delegate(Transform x, Transform y) {
			int result = x.name.CompareTo (y.name);
			if (result == 0)
				result = x.GetSiblingIndex ().CompareTo (y.GetSiblingIndex ());
			return result;
		}
		);

		for (int i = 0; i < children.Count; i++) {
			Transform child = children [i];
			if (child.GetSiblingIndex () != i) {
				child.SetSiblingIndex (i);
				this.Log ("Update {0} siblingIndex => {1}", child.name, i);
			}
		}
	}
#endif
}
