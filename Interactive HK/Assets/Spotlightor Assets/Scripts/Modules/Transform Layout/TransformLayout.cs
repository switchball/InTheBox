using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
public abstract class TransformLayout : MonoBehaviour
{
	public enum LayoutPlane
	{
		XY,
		XZ,
		YZ,
	}

	public enum LayoutAxis
	{
		X,
		Y,
		Z,
	}

	public bool includeInactives = false;

	void Update ()
	{
		if (!Application.isPlaying)
			UpdateLayout ();
	}

	public void UpdateLayout ()
	{
		List<Transform> childrenToLayout = new List<Transform> ();
		for (int i = 0; i < transform.childCount; i++) {
			Transform childTransform = transform.GetChild (i);
			if (childTransform.gameObject.activeSelf || includeInactives) 
				childrenToLayout.Add (childTransform);
		}
		if (childrenToLayout.Count > 0)
			UpdateLayoutForChildTransforms (childrenToLayout);
	}

	protected abstract void UpdateLayoutForChildTransforms (List<Transform> childTransforms);
}
