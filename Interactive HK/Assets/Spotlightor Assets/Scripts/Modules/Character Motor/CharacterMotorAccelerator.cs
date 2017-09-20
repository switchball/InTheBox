using UnityEngine;
using System.Collections;

[RequireComponent (typeof(EnhancedCharacterController))]
public class CharacterMotorAccelerator : MonoBehaviour
{
	public float drag = 0.1f;
	public float groundFrictionAcceleration = 1;
	private Vector3 speed = Vector3.zero;
	private Vector3 acceleration = Vector3.zero;

	void OnEnable ()
	{
		GetComponent<EnhancedCharacterController> ().BeforeUpdate += BeforeUpdate;
	}

	void OnDisable ()
	{
		GetComponent<EnhancedCharacterController> ().BeforeUpdate -= BeforeUpdate;
	}

	public void ApplyAcceleration (Vector3 a)
	{
		acceleration += a;
	}

	private void BeforeUpdate (EnhancedCharacterController enhancedController)
	{
		if (enhancedController.IsGrounded)
			speed.y = 0;
		
		speed += acceleration * Time.deltaTime;
		if (speed != Vector3.zero) {
//			if (enhancedController.IsGrounded) {
//				float totalSpeedY = enhancedController.SpeedY + speed.y;
//				if (totalSpeedY < 0)
//					speed.y = 0;
//			}

			Vector3 decceleration = Vector3.zero;
			decceleration += -speed * drag;
			if (enhancedController.IsGrounded)
				decceleration += -speed.normalized * groundFrictionAcceleration;

			Vector3 d = decceleration * Time.deltaTime;
			if (d.magnitude > speed.magnitude)
				speed = Vector3.zero;
			else
				speed += d;

			enhancedController.Move (speed * Time.deltaTime);
		}

//		this.Log ("SpeedY = {0}", speed.y);

		acceleration = Vector3.zero;
	}
}
