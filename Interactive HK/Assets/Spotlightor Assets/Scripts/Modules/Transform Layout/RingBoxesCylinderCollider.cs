using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingBoxesCylinderCollider : TransformLayout
{
	public Vector3 center = Vector3.zero;
	public float radius = 2;
	public float height = 3;
	public bool twoBoxesPerChild = false;

	protected override void UpdateLayoutForChildTransforms (List<Transform> childTransforms)
	{
		List<BoxCollider> boxColliders = new List<BoxCollider> ();
		foreach (Transform child in childTransforms) {
			BoxCollider boxCollider = child.GetComponent<BoxCollider> ();
			if (boxCollider != null)
				boxColliders.Add (boxCollider);
		}

		float childDeltaAngle = 0;
		int boxCollidersCount = boxColliders.Count;
		if (twoBoxesPerChild)
			boxCollidersCount *= 2;
		if (boxColliders.Count > 1)
			childDeltaAngle = 180 / (float)boxCollidersCount;

		float centreAngle = 0;
		Vector3 boxSize = new Vector3 (radius * Mathf.Sin (childDeltaAngle * 0.5f * Mathf.Deg2Rad) * 2, height, radius * Mathf.Cos (childDeltaAngle * 0.5f * Mathf.Deg2Rad) * 2);
		for (int i = 0; i < boxColliders.Count; i++) {
			BoxCollider box = boxColliders [i];

			box.transform.localPosition = center;
			box.transform.localEulerAngles = new Vector3 (0, centreAngle, 0);

			box.center = Vector3.zero;
			box.size = boxSize;

			if (twoBoxesPerChild) {
				BoxCollider[] allBoxes = box.GetComponents<BoxCollider> ();
				if (allBoxes.Length >= 2) {
					BoxCollider anotherBox = allBoxes [1];
					anotherBox.center = Vector3.zero;
					anotherBox.size = new Vector3 (boxSize.z, boxSize.y, boxSize.x);
				}
			}
			
			centreAngle += childDeltaAngle;
		}
	}
}
