using UnityEngine;
using System.Collections;

public class ScaleTransitionWidget : TransitionWidget
{
	public Vector3 scaleIn = Vector3.one;
	public Vector3 scaleOut = Vector3.zero;

	public override void UpdateTransitionProgress (float progress)
	{
		transform.localScale = Vector3.LerpUnclamped (scaleOut, scaleIn, progress);
	}

	void Reset ()
	{
		scaleIn = transform.localScale;
	}
}
