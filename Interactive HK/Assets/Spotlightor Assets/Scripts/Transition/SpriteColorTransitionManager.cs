using UnityEngine;
using System.Collections;

public class SpriteColorTransitionManager : ValueTransitionManager
{
	public Color colorIn = Color.white;
	public Color colorOut = new Color (1, 1, 1, 0);

	protected override void OnProgressValueUpdated (float progress)
	{
		GetComponent<SpriteRenderer> ().color = Color.Lerp (colorOut, colorIn, progress);
	}
}
