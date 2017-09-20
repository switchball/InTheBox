using UnityEngine;
using System.Collections;

public class TransformScaleShakeVisualizer : ShakeVisualizer {

	public float intensityToScaleOffset = 0.1f;
	private Vector3 defaultLocalScale = Vector3.one;
	
	void Awake ()
	{
		defaultLocalScale = transform.localScale;
	}
	
	protected override void VisualizeShake (Vector3 shakeStrength)
	{
		transform.localScale = defaultLocalScale + shakeStrength * intensityToScaleOffset;
	}
}
