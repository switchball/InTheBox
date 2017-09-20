using UnityEngine;
using System.Collections;

[System.Serializable]
public class SmoothFloat : SmoothValue<float>
{
	private float valueChangeVelocity = 0;

	public SmoothFloat (float followSmoothTime, float inertiaSmoothTime, float defaultValue, float maxSpeed) : base (followSmoothTime, inertiaSmoothTime, defaultValue, maxSpeed)
	{
	}

	protected override void OnValueSet ()
	{
		valueChangeVelocity = 0;
	}

	protected override float SmoothDampTo (float currentValue, float targetValue, float smoothTime, float deltaTime)
	{
		if (smoothTime > 0)
			return Mathf.SmoothDamp (currentValue, targetValue, ref valueChangeVelocity, smoothTime, maxSpeed, deltaTime);
		else {
			valueChangeVelocity = 0;
			return targetValue;
		}
	}

	public static implicit operator float (SmoothFloat smoothFloat)
	{
		return smoothFloat.Value;
	}

}
