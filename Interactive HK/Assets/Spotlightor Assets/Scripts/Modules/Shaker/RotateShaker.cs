using UnityEngine;
using System.Collections;

public class RotateShaker : Shaker {

	public float intensityToRotation = 0.1f;
	private Vector3 defaultEulerAngles = Vector3.zero;

	void Awake ()
	{
		defaultEulerAngles = transform.localEulerAngles;
	}

	public override void Shake (Vector3 intensity)
	{
		transform.localEulerAngles = defaultEulerAngles + intensity * intensityToRotation;
	}
}
