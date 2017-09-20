using UnityEngine;
using System.Collections;

[System.Serializable]
public class SmoothVector3 : SmoothValue<Vector3>
{
	private Vector3 valueChangeVelocity = Vector3.zero;

	public float X { 
		get { return Value.x; }
		set { Value = new Vector3 (value, Value.y, Value.z); }
	}

	public float Y { 
		get { return Value.y; }
		set { Value = new Vector3 (Value.x, value, Value.z); }
	}

	public float Z { 
		get { return Value.z; }
		set { Value = new Vector3 (Value.x, Value.y, value); }
	}

	public SmoothVector3 (float followSmoothTime, float inertiaSmoothTime, Vector3 defaultValue, float maxSpeed):base(followSmoothTime, inertiaSmoothTime, defaultValue, maxSpeed)
	{
	}
	
	protected override void OnValueSet ()
	{
		valueChangeVelocity = Vector3.zero;
	}
	
	protected override Vector3 SmoothDampTo (Vector3 currentValue, Vector3 targetValue, float smoothTime, float deltaTime)
	{
		return Vector3.SmoothDamp (currentValue, targetValue, ref valueChangeVelocity, smoothTime, maxSpeed, deltaTime);
	}
	
	public static implicit operator Vector3 (SmoothVector3 smoothVector3)
	{
		return smoothVector3.Value;
	}
}
