using UnityEngine;
using System.Collections;

[RequireComponent (typeof(EnhancedCharacterController))]
public class CharacterMotorForwardWalker : FunctionalMonoBehaviour
{
	[System.Serializable]
	public class PhysicsSettings : System.ICloneable
	{
		[Header ("Speed & Acceleration")]
		public float speed = 5;
		public float groundAcceleration = 30f;
		public float groundDeceleration = 100000f;
		public float airAcceleration = 10f;
		public float airDeceleration = 10f;
		[Header ("Dir & Turning")]
		public float slowTurnSpeed = 800;
		public float slowTurnDeltaAngle = 22.5f;
		public float fastTurnSpeed = 1800;
		public float fastTurnDeltaAngle = 45;
		public float brakeTurnAngle = 120f;
		public float slowFaceTurnDeltaAngle = 5;
		public float slowFaceTurnSpeed = 600;
		public float fastFaceTurnDeltaAngle = 45;
		public float fastFaceTurnSpeed = 1200;
		[Range (0, 1)]
		public float airTurnSpeedScale = 0.8f;

		public object Clone ()
		{
			return this.MemberwiseClone ();
		}

		public void Lerp (PhysicsSettings a, PhysicsSettings b, float t)
		{
			speed = Mathf.Lerp (a.speed, b.speed, t);
			groundAcceleration = Mathf.Lerp (a.groundAcceleration, b.groundAcceleration, t);
			groundDeceleration = Mathf.Lerp (a.groundDeceleration, b.groundDeceleration, t);
			airAcceleration = Mathf.Lerp (a.airAcceleration, b.airAcceleration, t);
			slowTurnSpeed = Mathf.Lerp (a.slowTurnSpeed, b.slowTurnSpeed, t);
			slowTurnDeltaAngle = Mathf.Lerp (a.slowTurnDeltaAngle, b.slowTurnDeltaAngle, t);
			fastTurnSpeed = Mathf.Lerp (a.fastTurnSpeed, b.fastTurnSpeed, t);
			fastTurnDeltaAngle = Mathf.Lerp (a.fastTurnDeltaAngle, b.fastTurnDeltaAngle, t);
			brakeTurnAngle = Mathf.Lerp (a.brakeTurnAngle, b.brakeTurnAngle, t);
			airTurnSpeedScale = Mathf.Lerp (a.airTurnSpeedScale, b.airTurnSpeedScale, t);

			fastFaceTurnDeltaAngle = Mathf.Lerp (a.fastFaceTurnDeltaAngle, b.fastFaceTurnDeltaAngle, t);
			fastFaceTurnSpeed = Mathf.Lerp (a.fastFaceTurnSpeed, b.fastFaceTurnSpeed, t);
			slowFaceTurnDeltaAngle = Mathf.Lerp (a.slowFaceTurnDeltaAngle, b.slowFaceTurnDeltaAngle, t);
			slowFaceTurnSpeed = Mathf.Lerp (a.slowFaceTurnSpeed, b.slowFaceTurnSpeed, t);
		}
	}

	public class TurningInfo //TODO: Move it to a special smooth float
	{
		public float turnStartDeltaAngle = 10;
		public float turnSpeed = 10;
		private bool isTurning = false;
		private float turnStartTime = 0;
		private float turnEndTime = 0;

		public bool IsTurning {
			get { return isTurning; }
			set {
				if (isTurning != value) {
					isTurning = value;
					if (isTurning)
						turnStartTime = turnEndTime = Time.time;
					else
						turnEndTime = Time.time;
				}
			}
		}

		public float LastTurnTime{ get { return turnEndTime - turnStartTime; } }
	}

	public PhysicsSettings physicsSettings;
	public int fixedDirectionsCount = 0;
	public bool brakeTurnInAir = false;

	[Range (0, 1), Header ("To implement move along jump start speed in air")]
	public float airTurnForwardStrength = 0.0f;

	[Header ("To help jump without running to jump further")]
	public float airPhysicsDelay = 0.1f;
	public float airPhysicsFadeTime = 0.1f;
	private float forwardStrength = 0;
	private Vector3 speedDir = Vector3.forward;
	private Vector3 targetForward = Vector3.forward;
	private EnhancedCharacterController enhancedController;
	private TurningInfo speedTurningInfo = new TurningInfo ();
	private TurningInfo faceTurningInfo = new TurningInfo ();

	public bool IsBrakeTurning{ get; private set; }

	public float ForwardStrength {
		get { return forwardStrength; }
		set { forwardStrength = Mathf.Clamp (value, 0, 1); }
	}

	public Vector3 TargetForward {
		get { return targetForward; }
		set {
			value.y = 0;
			if (value != Vector3.zero) {
				if (fixedDirectionsCount <= 0)
					targetForward = value.normalized;
				else {
					float dirSplitAngle = 360f / (float)fixedDirectionsCount;
					float targetAngle = Mathf.Atan2 (value.x, value.z) * Mathf.Rad2Deg;
					targetAngle = (float)Mathf.RoundToInt (targetAngle / dirSplitAngle) * dirSplitAngle;
					targetForward = Quaternion.Euler (0, targetAngle, 0) * Vector3.forward;
				}
			}
		}
	}

	private float AirPhysicsIntensity {
		get {
			float airPhysicsIntensity = 0;
			if (!enhancedController.IsNearGrounded) {
				float airTime = Time.time - enhancedController.LastLeaveGroundTime;
				if (airTime <= airPhysicsDelay)
					airPhysicsIntensity = 0;
				else if (airTime < airPhysicsDelay + airPhysicsFadeTime) {
					if (airPhysicsFadeTime > 0)
						airPhysicsIntensity = (airTime - airPhysicsDelay) / airPhysicsFadeTime;
				} else
					airPhysicsIntensity = 1;
			}
			return airPhysicsIntensity;
		}
	}

	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		if (forTheFirstTime)
			enhancedController = GetComponent<EnhancedCharacterController> ();
		
		targetForward = speedDir = transform.forward;
		enhancedController.BeforeUpdate += HandleBeforeUpdate;
		enhancedController.AfterApplyMove += HandleAfterApplyMove;
	}

	protected override void OnBecameUnFunctional ()
	{
		enhancedController.BeforeUpdate -= HandleBeforeUpdate;
		enhancedController.AfterApplyMove -= HandleAfterApplyMove;
	}

	void HandleBeforeUpdate (EnhancedCharacterController enhancedController)
	{
		UpdateSpeed ();
		UpdateFacing ();
	}

	private void UpdateSpeed ()
	{
//		this.Log ("Side Colliding = {0}", (enhancedController.TotalCollisionFlags & CollisionFlags.Sides) != 0);
		
		float currentSpeedAngle = Mathf.Atan2 (speedDir.x, speedDir.z) * Mathf.Rad2Deg;
		float targetForwardAngle = Mathf.Atan2 (targetForward.x, targetForward.z) * Mathf.Rad2Deg;
		float angleSpeedToTarget = Mathf.DeltaAngle (currentSpeedAngle, targetForwardAngle);

		if (angleSpeedToTarget != 0) {
			if (speedTurningInfo.IsTurning == false || speedTurningInfo.turnStartDeltaAngle < Mathf.Abs (angleSpeedToTarget)) {
				speedTurningInfo.IsTurning = true;
				speedTurningInfo.turnStartDeltaAngle = Mathf.Abs (angleSpeedToTarget);
				float slowOrFastTurn = Mathf.InverseLerp (physicsSettings.slowTurnDeltaAngle, physicsSettings.fastTurnDeltaAngle, Mathf.Abs (angleSpeedToTarget));
				speedTurningInfo.turnSpeed = Mathf.Lerp (physicsSettings.slowTurnSpeed, physicsSettings.fastTurnSpeed, slowOrFastTurn);

				if (!IsBrakeTurning)
					IsBrakeTurning = Mathf.Abs (angleSpeedToTarget) >= physicsSettings.brakeTurnAngle;
			}
		}
			
		float turnSpeed = speedTurningInfo.turnSpeed;

		if (!enhancedController.IsNearGrounded)
			turnSpeed *= Mathf.Lerp (1, physicsSettings.airTurnSpeedScale, AirPhysicsIntensity);

		float deltaRotateAngle = turnSpeed * Time.deltaTime;

		bool reachTargetForward = deltaRotateAngle >= Mathf.Abs (angleSpeedToTarget);
		if (!reachTargetForward) {
			if (angleSpeedToTarget < 0)
				deltaRotateAngle = -deltaRotateAngle;
			currentSpeedAngle += deltaRotateAngle;
			speedDir = Quaternion.Euler (0, currentSpeedAngle, 0) * Vector3.forward;
		} else {
			speedDir = TargetForward;
			speedTurningInfo.IsTurning = false;
		}

		if (IsBrakeTurning) {
			if (enhancedController.IsNearGrounded || brakeTurnInAir)
				enhancedController.SpeedX = enhancedController.SpeedZ = 0;

			if (reachTargetForward)
				IsBrakeTurning = false;
		} else {
			float targetForwardSpeed = ForwardStrength * physicsSettings.speed;
			float currentForwardSpeed = new Vector3 (enhancedController.SpeedX, 0, enhancedController.SpeedZ).magnitude;

			if (!enhancedController.IsNearGrounded && ForwardStrength < airTurnForwardStrength)
				targetForwardSpeed = currentForwardSpeed;

			float acceleration = Mathf.Lerp (physicsSettings.groundAcceleration, physicsSettings.airAcceleration, AirPhysicsIntensity);
			float decceleration = Mathf.Lerp (physicsSettings.groundDeceleration, physicsSettings.airDeceleration, AirPhysicsIntensity);

			if (currentForwardSpeed < targetForwardSpeed)
				currentForwardSpeed = Mathf.Min (currentForwardSpeed + acceleration * Time.deltaTime, targetForwardSpeed);
			else if (currentForwardSpeed > targetForwardSpeed)
				currentForwardSpeed = Mathf.Max (currentForwardSpeed - decceleration * Time.deltaTime, targetForwardSpeed);

			enhancedController.SpeedX = speedDir.x * currentForwardSpeed;
			enhancedController.SpeedZ = speedDir.z * currentForwardSpeed;
		}

//		if (speedTurningInfo.IsTurning)
//			this.Log ("Speed Turn with speed {0:0.0}, startDeltaAngle = {1:0.0}", speedTurningInfo.turnSpeed, speedTurningInfo.turnStartDeltaAngle);
	}

	private void UpdateFacing ()
	{
		float targetForwardAngle = Mathf.Atan2 (targetForward.x, targetForward.z) * Mathf.Rad2Deg;
		float currentFaceAngle = Mathf.Atan2 (enhancedController.Forward.x, enhancedController.Forward.z) * Mathf.Rad2Deg;
		float angleFaceToTarget = Mathf.DeltaAngle (currentFaceAngle, targetForwardAngle);

		if (angleFaceToTarget != 0) {
			if (faceTurningInfo.IsTurning == false || faceTurningInfo.turnStartDeltaAngle < Mathf.Abs (angleFaceToTarget)) {
				faceTurningInfo.IsTurning = true;
				faceTurningInfo.turnStartDeltaAngle = Mathf.Abs (angleFaceToTarget);
				float slowOrFastTurn = Mathf.InverseLerp (physicsSettings.slowFaceTurnDeltaAngle, physicsSettings.fastFaceTurnDeltaAngle, Mathf.Abs (angleFaceToTarget));
				faceTurningInfo.turnSpeed = Mathf.Lerp (physicsSettings.slowFaceTurnSpeed, physicsSettings.fastFaceTurnSpeed, slowOrFastTurn);
			}
		}

		float faceTurnSpeed = faceTurningInfo.turnSpeed;
		if (!enhancedController.IsNearGrounded)
			faceTurnSpeed *= physicsSettings.airTurnSpeedScale;

		float deltaFaceRotateAngle = faceTurnSpeed * Time.deltaTime;


		bool faceReachTargetForward = deltaFaceRotateAngle >= Mathf.Abs (angleFaceToTarget);
		if (!faceReachTargetForward) {
			if (angleFaceToTarget < 0)
				deltaFaceRotateAngle = -deltaFaceRotateAngle;

			currentFaceAngle += deltaFaceRotateAngle;
			enhancedController.Forward = Quaternion.Euler (0, currentFaceAngle, 0) * Vector3.forward;
		} else {
			enhancedController.Forward = TargetForward;
			faceTurningInfo.IsTurning = false;
		}

//		if (faceTurningInfo.IsTurning)
//			this.Log ("Face Turn with speed {0:0.0}, startDeltaAngle = {1:0.0}", faceTurningInfo.turnSpeed, faceTurningInfo.turnStartDeltaAngle);
	}

	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (Mathf.Abs (hit.moveDirection.y) < 0.3f)
			ReduceWalkSpeedBySideHit (hit);
	}

	private void ReduceWalkSpeedBySideHit (ControllerColliderHit hit)
	{
		Vector3 dirToHitPoint = hit.point - enhancedController.Center;
		dirToHitPoint.y = 0;
		dirToHitPoint.Normalize ();

		if (dirToHitPoint != Vector3.zero) {
			Vector3 hitReducedSpeed = Vector3.Project (new Vector3 (enhancedController.SpeedX, 0, enhancedController.SpeedZ), dirToHitPoint);
			enhancedController.Speed -= hitReducedSpeed;
		}
	}

	void HandleAfterApplyMove (EnhancedCharacterController enhancedController, Vector3 totalMotion)
	{
		// Other components (Moving Platform) may change forward in update.
		// The dir will only be fixed when updated using Forward property
		targetForward = enhancedController.transform.forward;
	}
}
