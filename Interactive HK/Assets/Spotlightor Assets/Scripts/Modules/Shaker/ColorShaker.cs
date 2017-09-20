using UnityEngine;
using System.Collections;

public class ColorShaker : Shaker
{
	public Color colorDefault = Color.white;
	public Color colorMaxShake = Color.black;
	public Vector3 axisMask = Vector3.right;
	private ColorDisplayer colorDisplayer;

	private ColorDisplayer ColorDisplayer {
		get {
			if (colorDisplayer == null) {
				colorDisplayer = GetComponent<ColorDisplayer> ();
				if (colorDisplayer == null)
					colorDisplayer = gameObject.AddComponent<ColorDisplayer> ();
			}
			return colorDisplayer;
		}
	}

	public override void Shake (Vector3 intensity)
	{
		intensity.Scale (axisMask);
		ColorDisplayer.Display (Color.Lerp (colorDefault, colorMaxShake, intensity.magnitude));
	}
}
