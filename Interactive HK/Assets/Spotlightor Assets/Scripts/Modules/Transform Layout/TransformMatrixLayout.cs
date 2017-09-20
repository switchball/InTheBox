using UnityEngine;
using System.Collections;

public class TransformMatrixLayout : TransformLayout
{
	public float offsetX = 1;
	public bool centeredX = false;
	public int xElementsCount = 3;
	public float offsetY = 1;
	public bool centeredY = false;
	public int yElementsCount = 3;
	public float offsetZ = 1;
	public bool centeredZ = false;
	
	protected override void UpdateLayoutForChildTransforms (System.Collections.Generic.List<Transform> childTransforms)
	{
		Vector3 localPosition = Vector3.zero;
		
		if (centeredX && childTransforms.Count > 1)
			localPosition.x = (float)(xElementsCount - 1) * -0.5f * offsetX;
		
		if (centeredY && childTransforms.Count > 1)
			localPosition.y = (float)((childTransforms.Count - 1) / xElementsCount) * -0.5f * offsetY;

		if (centeredZ && childTransforms.Count > 1)
			localPosition.z = (float)((childTransforms.Count - 1) / (xElementsCount * yElementsCount)) * -0.5f * offsetZ;
		
		int xRowIndex = 0;
		int yRowIndex = 0;
		float startX = localPosition.x;
		float startY = localPosition.y;
		foreach (Transform childTransform in childTransforms) {
			if (childTransform.localPosition != localPosition)
				childTransform.localPosition = localPosition;
			
			localPosition.x += offsetX;
			
			xRowIndex++;
			if (xRowIndex >= xElementsCount) {
				localPosition.y += offsetY;
				localPosition.x = startX;
				xRowIndex = 0;

				yRowIndex++;
				if (yRowIndex >= yElementsCount) {
					localPosition.z += offsetZ;
					localPosition.y = startY;
					yRowIndex = 0;
				}
			}
		}
	}
}
