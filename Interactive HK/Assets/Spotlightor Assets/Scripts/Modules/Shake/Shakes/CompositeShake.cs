using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompositeShake : Shake
{
	public List<Shake> shakes = new List<Shake> ();

	protected override Vector3 GetCurrentIntensity ()
	{
		Vector3 intensity = Vector3.zero;
		if (shakes.Count > 0) {
			int index = 0;
			while (index < shakes.Count) {
				Shake shake = shakes [index];
				if (shake != null) {
					intensity += shake.Intensity;
					index++;
				} else 
					shakes.RemoveAt (index);
			}
		}
		return intensity;
	}

	public void ClearShakes ()
	{
		shakes.Clear ();
	}

	public Shake InstantiateShake (Shake shakePrefab)
	{
		Shake shake = GameObject.Instantiate (shakePrefab) as Shake;
		shake.transform.SetParent (transform, false);
		shake.gameObject.SetActive (true);

		shakes.Add (shake);

		return shake;
	}

	void LateUpdate ()
	{
		bool hasAnyShakeShaking = false;
		int index = 0;
		while (index < shakes.Count) {
			Shake shake = shakes [index];
			if (shake != null) {
				hasAnyShakeShaking |= shake.IsShaking;
				index++;
			} else 
				shakes.RemoveAt (index);
		}

		if (IsShaking && !hasAnyShakeShaking)
			OnEnded ();
		else if (!IsShaking && hasAnyShakeShaking)
			OnStarted ();
	}

	void Reset ()
	{
		if (shakes == null || shakes.Count == 0) {
			shakes = new List<Shake> (GetComponentsInChildren<Shake> (true));
			shakes.Remove (this);
		}
	}
}
