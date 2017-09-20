using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RectMoveTransitionManager : ValueTransitionManager
{
	public Vector2 anchorPosIn;
	public Vector2 anchorPosOut;

	protected override void OnProgressValueUpdated (float progress)
	{
		GetComponent<RectTransform> ().anchoredPosition = MathAddons.LerpUncapped (anchorPosOut, anchorPosIn, progress);
	}
}
