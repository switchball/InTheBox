using UnityEngine;
using System.Collections;

public class StepRange : PropertyAttribute
{
	public float min = int.MinValue;
	public float max = int.MaxValue;
	public float step = 1;

	public StepRange (float min, float max, float step)
	{
		this.step = step;
		this.min = min;
		this.max = Mathf.Max (max, min);
	}
}
