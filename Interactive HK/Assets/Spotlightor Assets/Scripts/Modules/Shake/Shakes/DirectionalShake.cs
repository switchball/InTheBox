using UnityEngine;
using System.Collections;

public class DirectionalShake : TimedShake
{
	public Vector3 directionalIntensity = Vector3.up;
	public AnimationCurve intensityOverTime = new AnimationCurve (new Keyframe (0, 0), new Keyframe (0.05f, 1), new Keyframe (1, 0));

	protected override Vector3 GetIntensityAtTime (float time, float deltaTime)
	{
		return directionalIntensity * intensityOverTime.Evaluate (this.NormalizedTime);
	}
}
