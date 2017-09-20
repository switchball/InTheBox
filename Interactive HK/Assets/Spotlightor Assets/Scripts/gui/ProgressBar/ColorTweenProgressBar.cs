using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ColorDisplayer))]
public class ColorTweenProgressBar : ProgressBar
{
	[ColorUsage (true, true, 0, 8, 0.125f, 3)]
	public Color colorFrom = Color.black;
	[ColorUsage (true, true, 0, 8, 0.125f, 3)]
	public Color colorTo = Color.white;
	public AnimationCurve colorLerpByProgress = new AnimationCurve (new Keyframe (0, 0), new Keyframe (1, 1));

	protected override void UpdateProgressDisplay (float progress)
	{
		float lerp = colorLerpByProgress.Evaluate (progress);
		Color color = Color.Lerp (colorFrom, colorTo, lerp);
		GetComponent<ColorDisplayer> ().Display (color);
	}
}
