using UnityEngine;
using System.Collections;

public abstract class SmoothValue<T>
{
	public float followSmoothTime = 0.2f;
	public float inertiaSmoothTime = 0.5f;
	public float maxSpeed;
	public T defaultValue;
	private T value;
	private bool valueInitialized = false;

	public T Value {
		get {
			if (!valueInitialized) {
				this.value = defaultValue;
				valueInitialized = true;
			}
			return this.value;
		}
		set {
			this.value = value;

			OnValueSet ();

			if (!valueInitialized)
				valueInitialized = true;
		}
	}

	public SmoothValue ()
	{
		
	}
	
	public SmoothValue (float followSmoothTime, float inertiaSmoothTime, T defaultValue, float maxSpeed)
	{
		this.followSmoothTime = followSmoothTime;
		this.inertiaSmoothTime = inertiaSmoothTime;
		this.defaultValue = this.Value = defaultValue;
		this.maxSpeed = maxSpeed;
	}

	protected abstract void OnValueSet ();

	public void FollowToTargetValue (T targetValue)
	{
		FollowToTargetValue (targetValue, Time.deltaTime);
	}

	public void FollowToTargetValue (T targetValue, float deltaTime)
	{
		this.value = SmoothDampTo (this.Value, targetValue, followSmoothTime, deltaTime);
	}
	
	public void InertiaToDefaultValue ()
	{
		InertiaToDefaultValue (Time.deltaTime);
	}

	public void InertiaToDefaultValue (float deltaTime)
	{
		this.value = SmoothDampTo (this.Value, defaultValue, inertiaSmoothTime, deltaTime);
	}

	protected abstract T SmoothDampTo (T currentValue, T targetValue, float smoothTime, float deltaTime);
}
