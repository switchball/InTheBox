using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShakeSource : EnhancedMonoBehaviour
{
	public static List<ShakeSource> globalInstances = new List<ShakeSource> ();

	[System.Serializable]
	public class Settings3D
	{
		public float minDistance = 5;
		public float maxDistance = 20;
	}

	public ShakeClip shakeClip;
	public ShakeChannel channel;
	public bool playOnAwake = true;
	public bool loop = false;
	public float intensityScale = 1;
	public float lengthScale = 1;

	public bool isGlobal = true;

	public bool is3D = true;
	public Settings3D settings3D;

	private float playTime = 0;

	public Vector3 ShakeIntensity{ get; private set; }

	public bool IsPlaying{ get; private set; }

	public float PlayTime { get { return playTime; } }

	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		if (isGlobal)
			globalInstances.Add (this);

		if (playOnAwake)
			Play ();
	}

	protected override void OnBecameUnFunctional ()
	{
		if (isGlobal)
			globalInstances.Remove (this);
	}

	void OnDestroy ()
	{
		globalInstances.Remove (this);
	}

	public void Play ()
	{
		IsPlaying = true;
		shakeClip.OnPlayBy (this);
	}

	public void Stop ()
	{
		ShakeIntensity = Vector3.zero;
		playTime = 0;
		IsPlaying = false;
		shakeClip.OnStopBy (this);
	}

	void Update ()
	{
		if (IsPlaying) {
			playTime += Time.deltaTime / lengthScale;
			if (playTime <= shakeClip.length || loop) {
				if (playTime >= shakeClip.length)
					playTime %= shakeClip.length;

				ShakeIntensity = shakeClip.Sample (playTime, this);
				ShakeIntensity *= intensityScale;
			} else
				Stop ();
		}
	}

	public Vector3 GetShakeIntensityAtPosition (Vector3 position)
	{
		Vector3 intensity = this.ShakeIntensity;
		if (is3D)
			intensity *= GetShakeDistanceScaleAtPosition (position);
		return intensity;
	}

	public float GetShakeDistanceScaleAtPosition (Vector3 position)
	{
		float shakeDistanceScale = 1;
		float distance = Vector3.Distance (position, transform.position);
		if (distance > settings3D.maxDistance)
			shakeDistanceScale = 0;
		else if (distance > settings3D.minDistance)
			shakeDistanceScale = Mathf.InverseLerp (settings3D.maxDistance, settings3D.minDistance, distance);
		return shakeDistanceScale;
	}

	public void PlayInstance ()
	{
		PlayInstance (this.shakeClip);
	}

	public void PlayInstance (ShakeClip shakeClip)
	{
		PlayInstance (shakeClip, this.intensityScale, this.lengthScale);
	}

	public void PlayInstance (ShakeClip shakeClip, float intensityScale, float speedScale)
	{
		if (shakeClip != null) {
			ShakeSource source = gameObject.AddComponent<ShakeSource> ();
			source.shakeClip = shakeClip;
			source.channel = channel;
			source.playOnAwake = true;
			source.loop = false;
			source.intensityScale = intensityScale;
			source.lengthScale = speedScale;
			source.is3D = this.is3D;
			source.settings3D = new Settings3D ();
			source.settings3D.maxDistance = this.settings3D.maxDistance;
			source.settings3D.minDistance = this.settings3D.minDistance;

			Destroy (source, shakeClip.length / speedScale);
		}
	}

	void OnDrawGizmosSelected ()
	{
		if (settings3D != null) {
			Gizmos.color = new Color (1f, 0.5f, 0.5f);
			Gizmos.DrawWireSphere (transform.position, settings3D.minDistance);
			Gizmos.DrawWireSphere (transform.position, settings3D.maxDistance);
		}
	}
}
