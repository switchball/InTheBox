using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxColliderLineLayout : TransformLayout
{
	public LayoutAxis axis = LayoutAxis.Y;
	public bool alignByColliderCenter = true;
	public bool inverse = false;

	protected override void UpdateLayoutForChildTransforms (List<Transform> childTransforms)
	{
		Vector3 localPos = Vector3.zero;
		foreach (Transform child in childTransforms) {
			BoxCollider boxCollider = child.GetComponent<BoxCollider> ();
			if (boxCollider != null) {
				Vector3 axisOffset = Vector3.zero;
				if (axis == LayoutAxis.X)
					axisOffset.x = boxCollider.size.x;
				else if (axis == LayoutAxis.Y)
					axisOffset.y = boxCollider.size.y;
				else if (axis == LayoutAxis.Z)
					axisOffset.z = boxCollider.size.z;

				if (inverse)
					axisOffset = -axisOffset;

				if (alignByColliderCenter)
					child.localPosition = localPos - boxCollider.center + 0.5f * axisOffset;
				else
					child.localPosition = localPos;

				localPos += axisOffset;
			}
		}
	}
}
