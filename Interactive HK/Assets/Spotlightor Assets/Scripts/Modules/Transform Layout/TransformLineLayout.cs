using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransformLineLayout : TransformLayout
{
	public LayoutAxis axis = LayoutAxis.X;
	public float offset = 1;
	public bool centered = false;

	protected override void UpdateLayoutForChildTransforms (List<Transform> childTransforms)
	{
		Vector3 localPosition = Vector3.zero;

		if (centered && childTransforms.Count > 1)
			localPosition.x = (float)(childTransforms.Count - 1) * -0.5f * offset;

		foreach (Transform childTransform in childTransforms) {
			Vector3 mappedPosition = MapLinePosVectorToAxis (localPosition, axis);
			if (childTransform.localPosition != mappedPosition)
				childTransform.localPosition = mappedPosition;

			localPosition.x += offset;
		}
	}

	private static Vector3 MapLinePosVectorToAxis (Vector3 linePosVector, LayoutAxis axis)
	{
		Vector3 result = linePosVector;
		if (axis == LayoutAxis.Y) {
			float temp = result.y;
			result.y = result.x;
			result.x = temp;
		} else if (axis == LayoutAxis.Z) {
			float temp = result.z;
			result.z = result.x;
			result.x = temp;
		}
		return result;
	}
}
