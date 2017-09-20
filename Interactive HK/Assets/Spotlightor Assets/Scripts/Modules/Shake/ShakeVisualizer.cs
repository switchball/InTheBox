using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ShakeVisualizer : MonoBehaviour
{
	public Shake shake;

	void Start ()
	{
		if (shake != null) {
			shake.Started += HandleShakeStarted;
			shake.Ended += HandleShakeEnded;

			if (shake.IsShaking)
				HandleShakeStarted (shake);
		}
	}

	void HandleShakeStarted (Shake shake)
	{
		StartCoroutine ("VisualizeShakeLoop");
	}
	
	void HandleShakeEnded (Shake shake)
	{
		StopCoroutine ("VisualizeShakeLoop");
		VisualizeShake (Vector3.zero);
	}

	private IEnumerator VisualizeShakeLoop ()
	{
		while (true) {
			VisualizeShake (shake.Intensity);

			yield return null;
		}
	}

	protected virtual void Reset ()
	{
		if (shake == null)
			shake = GetComponent<Shake> ();
	}

	protected abstract void VisualizeShake (Vector3 intensity);
}
