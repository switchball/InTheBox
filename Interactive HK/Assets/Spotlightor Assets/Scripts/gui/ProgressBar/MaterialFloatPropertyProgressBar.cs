using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class MaterialFloatPropertyProgressBar : ProgressBar
{
	public string propertyName = "_RotateProgress";
	public float emptyValue = 0;
	public float fullValue = 1;

	protected override void UpdateProgressDisplay (float progress)
	{
		GetComponent<Renderer>().material.SetFloat (propertyName, Mathf.Lerp (emptyValue, fullValue, progress));
	}
}
