using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransformRingLayout : TransformLayout
{
	public float startAngle = 0;
	public bool uniformDistribution = true;
	[HideByBooleanProperty ("uniformDistribution", true)]
	public float
		deltaAngle = 10;
	[HideByBooleanProperty ("uniformDistribution", false)]
	public float
		angleRange = 360;
	public Vector3 childBaseRotation = Vector3.zero;
	public float centreOffset = 0;
	public Vector3 offset = Vector3.zero;

	protected override void UpdateLayoutForChildTransforms (List<Transform> childTransforms)
	{
		float childDeltaAngle = 0;
		if (uniformDistribution) {
			if (childTransforms.Count > 1)
				childDeltaAngle = angleRange / (float)(childTransforms.Count);
		} else
			childDeltaAngle = this.deltaAngle;

		float centreAngle = startAngle;
		for (int i = 0; i < childTransforms.Count; i++) {
			Transform child = childTransforms [i];
			child.localPosition = Quaternion.Euler (0, centreAngle, 0) * Vector3.forward * centreOffset;
			child.localPosition += offset * (float)i;

			child.localEulerAngles = new Vector3 (0, centreAngle, 0) + childBaseRotation;
			centreAngle += childDeltaAngle;
		}
	}
}
