using UnityEngine;
using System.Collections;

public class StepperInt : PropertyAttribute
{
	public int min = int.MinValue;
	public int max = int.MaxValue;
	public int step = 1;

	public StepperInt ()
	{
		
	}

	public StepperInt (int step)
	{
		this.step = step;
	}

	public StepperInt (int step, int min, int max)
	{
		this.step = step;
		this.min = min;
		this.max = Mathf.Max (max, min);
	}
}
