using UnityEngine;
using System.Collections;

public class ClampAttribute : PropertyAttribute
{
	public float min = 0;
	public float max = 2;

	public ClampAttribute (float min, float max)
	{
		this.min = min;
		this.max = Mathf.Max (max, min);
	}
}
