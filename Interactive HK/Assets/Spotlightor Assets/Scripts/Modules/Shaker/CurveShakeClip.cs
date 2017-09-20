using UnityEngine;
using System.Collections;

public class CurveShakeClip : ShakeClip
{
	public Vector3 axisIntensity = Vector3.up;
	[SingleLineLabel ()]
	public AnimationCurve intensityScaleByNormalizedTime = new AnimationCurve (new Keyframe (0, 1), new Keyframe (1, 0));

	public override Vector3 Sample (float time, ShakeSource source)
	{
		float normalizedTime = Mathf.Clamp01 (time / this.length);
		return axisIntensity * intensityScaleByNormalizedTime.Evaluate (normalizedTime);
	}
}
