using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(EnhancedCharacterController))]
public class CharacterMotorMovingPlatformer : FunctionalMonoBehaviour
{
	[System.Serializable]
	public class MovingPlatformInfo
	{
		public string tag = "Moving Platform";
		public bool inheritVelocity = false;

		public MovingPlatformInfo (string tag, bool inheritVelocity)
		{
			this.tag = tag;
			this.inheritVelocity = inheritVelocity;
		}
	}
	// Before Slope Slide & Ground clamp, because they are based on total motion.
	public List<MovingPlatformInfo> movingPlatformInfos = new List<MovingPlatformInfo> (){ new MovingPlatformInfo ("Moving Platform", false) };
	public bool offsetAsTransformMovement = true;
	private bool inheritPlatformVelocity = false;
	private EnhancedCharacterController enhancedController;
	private Vector3 localPositionOnPlatform = Vector3.zero;
	private Vector3 localForwardOnPlatform = Vector3.forward;

	public Vector3 PlatformVelocity { get; private set; }

	public bool IsOnMovingPlatform{ get; private set; }

	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		if (forTheFirstTime)
			enhancedController = GetComponent<EnhancedCharacterController> ();
		
		enhancedController.BeforeApplyMove += HandleBeforeApplyMove;
		enhancedController.AfterApplyMove += HandleAfterApplyMove;
		enhancedController.GroundEntered += HandleGroundEntered;
		enhancedController.GroundExited += HandleGroundExited;
	}

	protected override void OnBecameUnFunctional ()
	{
		enhancedController.BeforeApplyMove -= HandleBeforeApplyMove;
		enhancedController.AfterApplyMove -= HandleAfterApplyMove;
		enhancedController.GroundEntered -= HandleGroundEntered;
		enhancedController.GroundExited -= HandleGroundExited;
	}

	void HandleGroundEntered (EnhancedCharacterController source, Collider ground)
	{
		bool groundIsMovingPlatform = false;
		foreach (MovingPlatformInfo movingPlatformInfo in movingPlatformInfos) {
			if (ground.CompareTag (movingPlatformInfo.tag)) {
				groundIsMovingPlatform = true;
				inheritPlatformVelocity = movingPlatformInfo.inheritVelocity;
				break;
			}
		}
		if (groundIsMovingPlatform) {
			localPositionOnPlatform = ground.transform.InverseTransformPoint (transform.position);
			localForwardOnPlatform = ground.transform.InverseTransformDirection (transform.forward);
			PlatformVelocity = Vector3.zero;

			IsOnMovingPlatform = true;
		} else {
			PlatformVelocity = Vector3.zero;
			IsOnMovingPlatform = false;
		}
	}

	void HandleGroundExited (EnhancedCharacterController source, Collider ground)
	{
		if (IsOnMovingPlatform) {
			IsOnMovingPlatform = false;
		}
	}

	void HandleBeforeApplyMove (EnhancedCharacterController source, Vector3 totalMotion)
	{
		if (IsOnMovingPlatform && enhancedController.Ground == null)
			IsOnMovingPlatform = false;

		if (IsOnMovingPlatform) {
			Collider ground = enhancedController.Ground;

			Vector3 inheritForward = ground.transform.TransformDirection (localForwardOnPlatform);
			inheritForward.y = 0;
			inheritForward.Normalize ();
			if (inheritForward != Vector3.zero && inheritForward != transform.forward) {
				float currentForwardAngle = Mathf.Atan2 (transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
				float inheritForwardAngle = Mathf.Atan2 (inheritForward.x, inheritForward.z) * Mathf.Rad2Deg;
				float deltaAngle = Mathf.DeltaAngle (currentForwardAngle, inheritForwardAngle);
				enhancedController.Forward = Quaternion.Euler (0, deltaAngle, 0) * enhancedController.Forward;
			}
			
			Vector3 platformDeltaMovement = ground.transform.TransformPoint (localPositionOnPlatform) - transform.position;
			if (platformDeltaMovement != Vector3.zero) {
				// If apply platform movement as Move.motion, moving up platform will make controller's total motion.y > 0, whitch make "isGrounded" totally not working.
				// So we apply platform movement as transform.position, make motion in relative space, so enable the "isGrounded".
				if (offsetAsTransformMovement)
					enhancedController.transform.position += platformDeltaMovement;
				else
					enhancedController.Move (platformDeltaMovement);
			}

			PlatformVelocity = platformDeltaMovement / Time.deltaTime;
		} else {
			if (inheritPlatformVelocity && PlatformVelocity != Vector3.zero && !enhancedController.IsNearGrounded) {
				enhancedController.Move (PlatformVelocity * Time.deltaTime);
			}
		}

	}

	void HandleAfterApplyMove (EnhancedCharacterController source, Vector3 totalMotion)
	{
		if (IsOnMovingPlatform) {
			Collider ground = enhancedController.Ground;

			localPositionOnPlatform = ground.transform.InverseTransformPoint (transform.position);
			localForwardOnPlatform = ground.transform.InverseTransformDirection (transform.forward);
		}
	}
}
