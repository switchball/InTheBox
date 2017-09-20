using UnityEngine;
using System.Collections;

public class TransformRotationShakeVisualizer : ShakeVisualizer
{
	public float intensityToRotation = 1f;
	private Quaternion defaultLocalRotation = Quaternion.identity;
	
	void Awake ()
	{
		defaultLocalRotation = transform.localRotation;
	}
	
	protected override void VisualizeShake (Vector3 shakeStrength)
	{
		transform.localRotation = defaultLocalRotation * Quaternion.Euler (shakeStrength * intensityToRotation);
	}
}
