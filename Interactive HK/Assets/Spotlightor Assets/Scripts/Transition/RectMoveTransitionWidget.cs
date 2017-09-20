using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class RectMoveTransitionWidget : TransitionWidget
{
	public Vector2 anchorPosIn;
	public Vector2 anchorPosOut;

	public override void UpdateTransitionProgress (float progress)
	{
		GetComponent<RectTransform> ().anchoredPosition = MathAddons.LerpUncapped (anchorPosOut, anchorPosIn, progress);
	}
}
