using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorTransitionManager : ValueTransitionManager
{
	[ColorUsage (true, true, 0, 8, 0.125f, 3)]
	public Color colorIn = Color.white;
	[ColorUsage (true, true, 0, 8, 0.125f, 3)]
	public Color colorOut = new Color (1, 1, 1, 0);

	private ColorDisplayer colorDisplayer;

	public ColorDisplayer ColorDisplayer {
		get {
			if (colorDisplayer == null) {
				colorDisplayer = GetComponent<ColorDisplayer> ();
				if (colorDisplayer == null)
					colorDisplayer = gameObject.AddComponent<ColorDisplayer> ();
			}
			return colorDisplayer;
		}
	}

	protected override void OnProgressValueUpdated (float progress)
	{
		ColorDisplayer.Display (Color.Lerp (colorOut, colorIn, progress));
	}

	void Reset ()
	{
		easeIn = iTween.EaseType.linear;
		easeOut = iTween.EaseType.linear;
	}
}
