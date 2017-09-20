using UnityEngine;
using System.Collections;

[System.Serializable]
public class ClampRangeFloat : ClampRange<float>
{
	public ClampRangeFloat (float min, float max):base(min,max)
	{
	}

	public override float Clamp (float value)
	{
		return Mathf.Clamp (value, min, max);
	}

	public override bool Contains (float value)
	{
		return value <= max && value >= min;
	}
}
