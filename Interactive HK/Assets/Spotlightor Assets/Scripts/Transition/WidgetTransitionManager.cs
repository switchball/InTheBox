using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WidgetTransitionManager : ValueTransitionManager
{
	private List<TransitionWidget> transitionWidgets;

	public List<TransitionWidget> TransitionWidgets {
		get {
			if (transitionWidgets == null)
				transitionWidgets = new List<TransitionWidget> (GetComponents<TransitionWidget> ());
			return transitionWidgets;
		}
	}

	protected override void OnProgressValueUpdated (float progress)
	{
		TransitionWidgets.ForEach (w => w.UpdateTransitionProgress (progress));
	}
}
