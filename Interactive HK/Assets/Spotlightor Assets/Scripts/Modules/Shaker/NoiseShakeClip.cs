using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoiseShakeClip : ShakeClip
{
	private const float PerlinNoiseStartRandomRange = 10000;
	public Vector3 axisIntensity = new Vector3 (0.5f, 0.5f, 0.5f);
	public float frequency = 30;
	[Range (0, 1)]
	public float
		fadeStartTimePercent = 0.6f;
	private Dictionary<ShakeSource, Vector4> seedByShakesources = new Dictionary<ShakeSource, Vector4> ();

	public override void OnPlayBy (ShakeSource source)
	{
		seedByShakesources [source] = new Vector4 (
			Random.value * PerlinNoiseStartRandomRange, 
			Random.value * PerlinNoiseStartRandomRange, 
			Random.value * PerlinNoiseStartRandomRange, 
			Random.value * PerlinNoiseStartRandomRange);
	}

	public override void OnStopBy (ShakeSource source)
	{
		seedByShakesources.Remove (source);
	}

	public override Vector3 Sample (float time, ShakeSource source)
	{
		Vector4 seed = Vector4.zero;
		seedByShakesources.TryGetValue (source, out seed);
		float noiseOffsetX = time * frequency;
		Vector3 intensity = Vector3.zero;
		
		if (axisIntensity.x != 0)
			intensity.x = GetPerlinNoiseValue (seed.x + noiseOffsetX, seed.w, axisIntensity.x);
		if (axisIntensity.y != 0)
			intensity.y = GetPerlinNoiseValue (seed.y + noiseOffsetX, seed.w, axisIntensity.y);
		if (axisIntensity.z != 0)
			intensity.z = GetPerlinNoiseValue (seed.z + noiseOffsetX, seed.w, axisIntensity.z);

		intensity *= GetFadeScaleAtTime(time);
		
		return intensity;
	}

	public float GetFadeScaleAtTime (float time)
	{
		float normalizedTime = Mathf.Clamp01 (time / length);
		float fadeScale = 1 - Mathf.InverseLerp (fadeStartTimePercent, 1, normalizedTime);
		return fadeScale;
	}

	private float GetPerlinNoiseValue (float x, float y, float intensity)
	{
		return (Mathf.PerlinNoise (x, y) * 2f - 1f) * intensity;
	}
}
