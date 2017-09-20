using UnityEngine;
using System.Collections;

public class ColorProgressBar : ProgressBar
{
	public Gradient progressColor;
	public bool inactiveOnEmptyProgress = false;
	private ColorDisplayer colorDisplayer;

	private ColorDisplayer MyColorDisplayer {
		get {
			if (colorDisplayer == null) {
				colorDisplayer = GetComponent<ColorDisplayer> ();
				if (colorDisplayer == null)
					colorDisplayer = gameObject.AddComponent<ColorDisplayer> ();
			}
			return colorDisplayer;
		}
	}

	protected override void UpdateProgressDisplay (float progress)
	{
		MyColorDisplayer.Display (progressColor.Evaluate (progress));

		if (inactiveOnEmptyProgress)
			gameObject.SetActive (progress > 0);
	}
}
