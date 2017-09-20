using UnityEngine;
using System.Collections;

public class OffsetShaker : Shaker
{
	public float intensityToOffset = 0.1f;
	private Vector3 defaultPosition = Vector3.zero;
	
	void Awake ()
	{
		defaultPosition = transform.localPosition;
	}

	public override void Shake (Vector3 intensity)
	{
		transform.localPosition = defaultPosition + intensity * intensityToOffset;
	}
}
