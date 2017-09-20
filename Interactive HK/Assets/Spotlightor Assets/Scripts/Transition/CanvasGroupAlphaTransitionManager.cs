using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupAlphaTransitionManager : ValueTransitionManager
{
	protected override void OnProgressValueUpdated (float progress)
	{
		GetComponent<CanvasGroup> ().alpha = progress;
	}

	protected override void DoTransitionIn (bool instant, StateTypes prevStateType)
	{
		// CanvasGroup.alpha will be set to 1 after Awake (bug), so we've to make sure alpha = 0 before transition in.
		if (prevStateType == StateTypes.Out)
			OnProgressValueUpdated (0);

		base.DoTransitionIn (instant, prevStateType);
	}
}
