using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompositeProgressBar : ProgressBar
{
	public List<ProgressBar> childProgressBars;

	protected override void UpdateProgressDisplay (float progress)
	{
		childProgressBars.ForEach (p => p.CurrentProgress = progress);
	}

	void Reset ()
	{
		if (childProgressBars == null || childProgressBars.Count == 0) {
			childProgressBars = new List<ProgressBar> (GetComponentsInChildren<ProgressBar> (true));
			childProgressBars.Remove (this);
		}
	}
}
