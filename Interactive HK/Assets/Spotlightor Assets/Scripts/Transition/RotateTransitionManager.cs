using UnityEngine;
using System.Collections;

public class RotateTransitionManager : ValueTransitionManager
{
	public Vector3 inEulerAngles = Vector3.zero;
	public Vector3 outEulerAngles = new Vector3 (0, 0, 360);

	protected override void OnProgressValueUpdated (float progress)
	{
		transform.localEulerAngles = outEulerAngles + (inEulerAngles - outEulerAngles) * progress;
	}

}
