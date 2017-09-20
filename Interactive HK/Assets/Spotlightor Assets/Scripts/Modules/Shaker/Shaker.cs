using UnityEngine;
using System.Collections;

public abstract class Shaker : MonoBehaviour
{
	public ShakeSensor sensor;

	void Update ()
	{
		Shake (sensor.ShakeIntensity);
	}

	public abstract void Shake (Vector3 intensity);

	void Rest ()
	{
		if (sensor == null)
			sensor = GetComponent<ShakeSensor> ();
	}
}
