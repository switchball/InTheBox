using UnityEngine;
using System.Collections;

public class ScaleShaker : Shaker
{
	public float intensityToScaleOffset = 0.1f;
	private Vector3 defaultLocalScale = Vector3.one;

	void Start ()
	{
		defaultLocalScale = transform.localScale;
	}

	public override void Shake (Vector3 intensity)
	{
		transform.localScale = defaultLocalScale + intensity * intensityToScaleOffset;
	}
}
