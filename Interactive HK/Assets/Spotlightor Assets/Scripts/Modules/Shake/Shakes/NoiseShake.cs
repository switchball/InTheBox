using UnityEngine;
using System.Collections;

public class NoiseShake : TimedShake
{
	private const float PerlinNoiseStartRandomRange = 10000;
	public Vector3 axisIntensity = new Vector3 (0.5f, 0.5f, 0.5f);
	public float frequency = 30;
	[Range(0,1)]
	public float
		fadeStartNormalizedTime = 0.6f;
	private Vector3 axisPerlinNoiseStartX = Vector3.zero;
	private float perlineNoiseStartY = 0;

	protected override void HookBeforeStarted ()
	{
		if (axisIntensity.x != 0)
			axisPerlinNoiseStartX.x = Random.Range (0, PerlinNoiseStartRandomRange);
		if (axisIntensity.y != 0)
			axisPerlinNoiseStartX.y = Random.Range (0, PerlinNoiseStartRandomRange);
		if (axisIntensity.z != 0)
			axisPerlinNoiseStartX.z = Random.Range (0, PerlinNoiseStartRandomRange);

		perlineNoiseStartY = Random.Range (0, PerlinNoiseStartRandomRange);
	}

	protected override Vector3 GetIntensityAtTime (float time, float deltaTime)
	{
		Vector3 intensity = Vector3.zero;
		float noiseOffsetX = time * frequency;
		float noiseY = perlineNoiseStartY;

		if (axisIntensity.x != 0)
			intensity.x = GetPerlinNoiseValue (axisPerlinNoiseStartX.x + noiseOffsetX, noiseY, axisIntensity.x);
		if (axisIntensity.y != 0)
			intensity.y = GetPerlinNoiseValue (axisPerlinNoiseStartX.y + noiseOffsetX, noiseY, axisIntensity.y);
		if (axisIntensity.z != 0)
			intensity.z = GetPerlinNoiseValue (axisPerlinNoiseStartX.z + noiseOffsetX, noiseY, axisIntensity.z);

		float fadeScale = 1 - Mathf.InverseLerp (fadeStartNormalizedTime, 1, this.NormalizedTime);
		intensity *= fadeScale;

		return intensity;
	}

	private float GetPerlinNoiseValue (float x, float y, float intensity)
	{
		return (Mathf.PerlinNoise (x, y) * 2f - 1f) * intensity;
	}
}
