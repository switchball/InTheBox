using UnityEngine;
using System.Collections;

public class TransformOffsetShakeVisualizer : ShakeVisualizer
{
	public float intensityToOffset = 0.1f;
	private Vector3 defaultPosition = Vector3.zero;

	void Awake ()
	{
		defaultPosition = transform.localPosition;
	}

	protected override void VisualizeShake (Vector3 shakeStrength)
	{
		transform.localPosition = defaultPosition + shakeStrength * intensityToOffset;
	}

}
