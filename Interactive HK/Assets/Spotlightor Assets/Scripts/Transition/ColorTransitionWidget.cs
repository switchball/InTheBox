using UnityEngine;
using System.Collections;

public class ColorTransitionWidget : TransitionWidget
{
	public Color colorIn = Color.white;
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

	public override void UpdateTransitionProgress (float progress)
	{
		ColorDisplayer.Display (Color.Lerp (colorOut, colorIn, progress));
	}
}
