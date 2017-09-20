using UnityEngine;
using System.Collections;

public class RotateTransitionWidget : TransitionWidget
{
	public Vector3 inEulerAngles = Vector3.zero;
	public Vector3 outEulerAngles = new Vector3 (0, 0, 360);

	public override void UpdateTransitionProgress (float progress)
	{
		transform.localEulerAngles = outEulerAngles + (inEulerAngles - outEulerAngles) * progress;
	}
}
