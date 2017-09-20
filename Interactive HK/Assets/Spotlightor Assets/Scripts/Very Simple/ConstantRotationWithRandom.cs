using UnityEngine;
using System.Collections;

public class ConstantRotationWithRandom : ConstantRotation
{
	public float rotationSpeedVaration = 30;
	public Vector3 axisRandomRotation = new Vector3 (30, 30, 30);

	protected override void Start ()
	{
		rotationSpeed += Random.Range (-rotationSpeedVaration, rotationSpeedVaration);

		Vector3 axisEulerRotation = axisRandomRotation;
		axisEulerRotation.x *= Random.Range (-1f, 1f);
		axisEulerRotation.y *= Random.Range (-1f, 1f);
		axisEulerRotation.z *= Random.Range (-1f, 1f);

		this.axis = Quaternion.Euler (axisEulerRotation) * this.axis;

		base.Start ();
	}
}
