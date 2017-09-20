using UnityEngine;
using System.Collections;

public abstract class TimedShake : Shake
{
	public float duration = 0.5f;
	public bool looping = false;
	public bool playOnAwake = true;
	public bool autoDestruct = true;
	private Vector3 intensity = Vector3.zero;

	public float NormalizedTime{ get; private set; }

	void OnEnable ()
	{
		if (playOnAwake)
			Play ();
	}

	public void Play ()
	{
		NormalizedTime = 0;

		StartCoroutine ("ShakeForTime");

		OnStarted ();
	}

	public void Stop ()
	{
		StopCoroutine ("ShakeForTime");

		intensity = Vector3.zero;

		OnEnded ();
	}

	private IEnumerator ShakeForTime ()
	{
		do {
			float timeElapsed = 0;
			float deltaTime = 0;
			NormalizedTime = 0;
			while (timeElapsed <= duration) {
				intensity = GetIntensityAtTime (timeElapsed, deltaTime);

				yield return null;

				timeElapsed += Time.deltaTime;
				deltaTime = Time.deltaTime;
				NormalizedTime = timeElapsed / duration;
			}
		} while(looping);

		NormalizedTime = 1;

		Stop ();

		if (autoDestruct)
			Destroy (gameObject);
	}

	protected abstract Vector3 GetIntensityAtTime (float time, float deltaTime);

	protected override Vector3 GetCurrentIntensity ()
	{
		return this.intensity;
	}

	void OnDisable ()
	{
		if (this.IsShaking)
			Stop ();
	}
}
