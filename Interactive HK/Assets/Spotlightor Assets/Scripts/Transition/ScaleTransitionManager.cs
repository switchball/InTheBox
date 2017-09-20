using UnityEngine;
using System.Collections;

public class ScaleTransitionManager : ValueTransitionManager
{
	public Vector3 scaleIn = Vector3.one;
	public Vector3 scaleOut = Vector3.zero;

	protected override void OnProgressValueUpdated (float progress)
	{
		transform.localScale = MathAddons.LerpUncapped (scaleOut, scaleIn, progress);
	}
	
	void Reset ()
	{
		scaleIn = transform.localScale;
	}
}
