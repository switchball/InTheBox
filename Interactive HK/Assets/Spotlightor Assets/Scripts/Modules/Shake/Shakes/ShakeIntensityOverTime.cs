using UnityEngine;
using System.Collections;

public class ShakeIntensityOverTime : ShakeDecorator<TimedShake>
{
	public AnimationCurve intensityScaleOverTime = new AnimationCurve (new Keyframe (0, 1), new Keyframe (1, 0, -1.414f, 0f));

	protected override Vector3 GetCurrentIntensity ()
	{
		return shake.Intensity * intensityScaleOverTime.Evaluate (shake.NormalizedTime);
	}
}
