using UnityEngine;
using System.Collections;

public class RotationProgressBar : ProgressBar
{
	public Vector3 emptyEulerAngles = Vector3.zero;
	public Vector3 fullEulerAngles = new Vector3 (0, 0, -360);

	protected override void UpdateProgressDisplay (float progress)
	{
		Vector3 eulerAngles = Vector3.Lerp (emptyEulerAngles, fullEulerAngles, progress);
		transform.localEulerAngles = eulerAngles;
	}
}
