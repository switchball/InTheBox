using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;

public class TransformGridLayout : TransformLayout
{
	public TransformLayout.LayoutPlane plane = TransformLayout.LayoutPlane.XY;
	public float offsetX = 1;
	public bool centeredX = false;
	[StepperInt (1, 1, int.MaxValue)]
	public int xElementsCount = 3;
	public float offsetY = 1;
	public bool centeredY = false;

	protected override void UpdateLayoutForChildTransforms (System.Collections.Generic.List<Transform> childTransforms)
	{
		Vector3 localPosition = Vector3.zero;

		if (centeredX && childTransforms.Count > 1)
			localPosition.x = (float)(xElementsCount - 1) * -0.5f * offsetX;

		if (centeredY && childTransforms.Count > 1)
			localPosition.y = (float)((childTransforms.Count - 1) / xElementsCount) * -0.5f * offsetY;

		int xRowIndex = 0;
		float rowStartX = localPosition.x;
		foreach (Transform childTransform in childTransforms) {
			Vector3 mappedLocalPosition = MapXyVectorToPlane (localPosition, plane);
			if (childTransform.localPosition != mappedLocalPosition)
				childTransform.localPosition = mappedLocalPosition;

			localPosition.x += offsetX;

			xRowIndex++;
			if (xRowIndex >= xElementsCount) {
				localPosition.y += offsetY;
				localPosition.x = rowStartX;
				xRowIndex = 0;
			}
		}
	}

	private static Vector3 MapXyVectorToPlane (Vector3 xyPlaneVector, TransformLayout.LayoutPlane targetPlane)
	{
		Vector3 result = xyPlaneVector;
		if (targetPlane == LayoutPlane.XZ) {
			float temp = result.z;
			result.z = result.y;
			result.y = temp;
		} else if (targetPlane == LayoutPlane.YZ) {
			float temp = result.z;
			result.z = result.x;
			result.x = temp;
		}
		return result;
	}
}
