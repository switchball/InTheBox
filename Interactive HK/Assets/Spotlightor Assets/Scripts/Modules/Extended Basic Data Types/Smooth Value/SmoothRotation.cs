using UnityEngine;
using System.Collections;

[System.Serializable]
public class SmoothRotation : SmoothValue<Vector3>
{
	public Quaternion ValueAsQuaternion { 
		get { return Quaternion.Euler (this.Value); } 
		set { this.Value = value.eulerAngles; } 
	}

	private float valueChangeVelocity = 0;

	public SmoothRotation (float followSmoothTime, float inertiaSmoothTime, Vector3 defaultEulerAngles, float maxSpeed):base(followSmoothTime, inertiaSmoothTime, defaultEulerAngles, maxSpeed)
	{
	}
	
	protected override void OnValueSet ()
	{
		valueChangeVelocity = 0;
	}

	public void FollowToTargetValue (Quaternion targetValue)
	{
		FollowToTargetValue (targetValue.eulerAngles, Time.deltaTime);
	}
	
	public void FollowToTargetValue (Quaternion targetValue, float deltaTime)
	{
		FollowToTargetValue (targetValue.eulerAngles, deltaTime);
	}
	
	protected override Vector3 SmoothDampTo (Vector3 currentValue, Vector3 targetValue, float smoothTime, float deltaTime)
	{
		Quaternion currentRotation = Quaternion.Euler (currentValue);
		Quaternion targetRotation = Quaternion.Euler (targetValue);
		float lerpSpeed = 1 / smoothTime;

		float t = lerpSpeed * deltaTime;
		Quaternion newRotation = Quaternion.Slerp (currentRotation, targetRotation, t);

		valueChangeVelocity = Quaternion.Angle (newRotation, currentRotation) / deltaTime;
		if (valueChangeVelocity > maxSpeed) {
			float maxSpeedPercent = maxSpeed / valueChangeVelocity;
			newRotation = Quaternion.Slerp (currentRotation, newRotation, maxSpeedPercent);
			valueChangeVelocity = Quaternion.Angle (newRotation, currentRotation) / deltaTime;
		}

		return newRotation.eulerAngles;
	}

	public static implicit operator Vector3 (SmoothRotation smoothRotation)
	{
		return smoothRotation.Value;
	}

	public static implicit operator Quaternion (SmoothRotation smoothRotation)
	{
		return Quaternion.Euler (smoothRotation.Value);
	}
}
