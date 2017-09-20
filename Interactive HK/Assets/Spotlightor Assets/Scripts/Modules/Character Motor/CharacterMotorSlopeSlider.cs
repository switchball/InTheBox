using UnityEngine;
using System.Collections;

[RequireComponent (typeof(EnhancedCharacterController))]
public class CharacterMotorSlopeSlider : MonoBehaviour
{
	[System.Serializable]
	public class SlopeSettings
	{
		[Range (0, 1f)]
		public float friction = 0.8f;
		[Range (0, 1f)]
		public float forwardCancel = 0.5f;
		public float forwardToClimbUp = 0.0f;
		public float minSpeedY = -10;
	}
	// Slide before Ground Clamp
	[Header ("Script Order: 4")]
	public float slideStartSlopeAngle = 46;
	public SlopeSettings slideStartSlopeSettings;
	public float slideMaxSlopeAngle = 70f;
	public SlopeSettings slideMaxSlopeSettings;
	public float slideStopSlopeAngle = 89;
	//TODO sliding speed should calculated based on speed along slope, not vertical speedY
	private float lastSlopeSpeedY = 0;
	private Vector3 lastGroundNormal = Vector3.up;
	private bool isSliding = false;

	public bool IsSliding {
		get{ return isSliding; }
		private set {
			if (isSliding != value) {
				isSliding = value;
				if (!isSliding)
					lastSlopeSpeedY = 0;
			}
		}
	}

	void OnEnable ()
	{
		GetComponent<EnhancedCharacterController> ().BeforeApplyMove += HandleBeforeApplyMove;
		GetComponent<EnhancedCharacterController> ().GroundEntered += HandleGroundEntered;
		GetComponent<EnhancedCharacterController> ().Floated += HandleFloated;
	}

	void OnDisable ()
	{
		GetComponent<EnhancedCharacterController> ().BeforeApplyMove -= HandleBeforeApplyMove;
		GetComponent<EnhancedCharacterController> ().GroundEntered -= HandleGroundEntered;
		GetComponent<EnhancedCharacterController> ().Floated -= HandleFloated;
	}

	void HandleGroundEntered (EnhancedCharacterController enhancedController, Collider ground)
	{
		if (IsSliding) {
			bool shouldStopSliding = true;
			if (ShouldEnhancedControllerSlide (enhancedController)) {
				Vector3 currentGroundNormal = enhancedController.GroundHitNormal;
				Vector3 lastXZ = new Vector3 (lastGroundNormal.x, 0, lastGroundNormal.z).normalized;
				Vector3 currentXZ = new Vector3 (currentGroundNormal.x, 0, currentGroundNormal.z).normalized;
				if (Vector3.Angle (lastXZ, currentXZ) < 15)
					shouldStopSliding = false;
			}
			if (shouldStopSliding)
				IsSliding = false;
		}
	}

	void HandleFloated (EnhancedCharacterController enhancedController, Collider ground)
	{
		if (IsSliding)
			enhancedController.SpeedY += lastSlopeSpeedY;
		
		IsSliding = false;
	}

	void HandleBeforeApplyMove (EnhancedCharacterController enhancedController, Vector3 totalMotion)
	{
		if (enhancedController.IsGrounded && enhancedController.SpeedY <= 0 && enhancedController.GroundHitNormal.y < 0.99f) {
			float slopeAngle = enhancedController.GroundSlopeAngle;

			float distanceToBelowCenter = Vector3.Distance (enhancedController.GroundHitPoint, enhancedController.BelowSphereCenter);
			distanceToBelowCenter = enhancedController.Radius;
			float maxSlopeChangeAngle = 15;

			float detectLength = distanceToBelowCenter / Mathf.Cos (Mathf.Deg2Rad * (slopeAngle + maxSlopeChangeAngle));
			detectLength += 0.1f;

//			Debug.DrawLine (enhancedController.BelowSphereCenter, enhancedController.BelowSphereCenter + Vector3.down * detectLength);

			RaycastHit hit;
			if (!Physics.Raycast (enhancedController.BelowSphereCenter, Vector3.down, out hit, detectLength, enhancedController.nearGroundLayerMask)) {
				slopeAngle = Mathf.Max (slideStartSlopeAngle, slopeAngle);
			}
				
			if (slopeAngle >= slideStartSlopeAngle && slopeAngle <= slideStopSlopeAngle) {
				Vector3 slopeNormal = enhancedController.GroundHitNormal;
				float slopeNormalXz = Mathf.Sin (slopeAngle * Mathf.Deg2Rad);
				slopeNormal = new Vector3 (slopeNormal.x, 0, slopeNormal.z).normalized * slopeNormalXz + Vector3.up * Mathf.Cos (slopeAngle * Mathf.Deg2Rad);

				lastSlopeSpeedY += Time.deltaTime * enhancedController.gravity;

				float slopeLerpT = Mathf.InverseLerp (slideStartSlopeAngle, slideMaxSlopeAngle, slopeAngle);

				float antiGravityPercent = Mathf.Lerp (slideStartSlopeSettings.friction, slideMaxSlopeSettings.friction, slopeLerpT);
				float antiGravityForce = -antiGravityPercent * enhancedController.gravity;
				lastSlopeSpeedY += Time.deltaTime * antiGravityForce;

				float minSpeedY = Mathf.Lerp (slideStartSlopeSettings.minSpeedY, slideMaxSlopeSettings.minSpeedY, slopeLerpT);
				lastSlopeSpeedY = Mathf.Max (minSpeedY, lastSlopeSpeedY);

				float slopeSlideSpeed = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * -lastSlopeSpeedY;
				
				lastGroundNormal = slopeNormal;
				Vector3 groundHitNormalXZ = slopeNormal;
				groundHitNormalXZ.y = 0;
				float groundHitNormalXZLength = groundHitNormalXZ.magnitude;
				Vector3 slideDownNormal = slopeNormal;
				slideDownNormal.y = -Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * groundHitNormalXZLength;
				slideDownNormal.Normalize ();

				Vector3 antiSlideHorizontalDir = -groundHitNormalXZ.normalized;
				Vector3 motionXZ = new Vector3 (totalMotion.x, 0, totalMotion.z);
				if (Vector3.Angle (antiSlideHorizontalDir, motionXZ) < 90) {
					float forwardCancel = Mathf.Lerp (slideStartSlopeSettings.forwardCancel, slideMaxSlopeSettings.forwardCancel, slopeLerpT);
					Vector3 antiSlideHorizontalMotion = Vector3.Project (motionXZ, antiSlideHorizontalDir);
					enhancedController.Move (-antiSlideHorizontalMotion * forwardCancel);

					float forwardToClimbUp = Mathf.Lerp (slideStartSlopeSettings.forwardToClimbUp, slideMaxSlopeSettings.forwardToClimbUp, slopeLerpT);
					float forwardLength = antiSlideHorizontalMotion.magnitude;
					float climbUp = forwardLength * forwardToClimbUp;
					lastSlopeSpeedY += climbUp;
					lastSlopeSpeedY = Mathf.Min (0, lastSlopeSpeedY);
				}

				enhancedController.Move (slideDownNormal * slopeSlideSpeed * Time.deltaTime);

				IsSliding = true;
			} else
				IsSliding = false;
		} else
			IsSliding = false;
	}

	private bool ShouldEnhancedControllerSlide (EnhancedCharacterController enhancedController)
	{
		bool shouldSlide = false;
		if (enhancedController.IsGrounded && enhancedController.SpeedY <= 0 && enhancedController.GroundHitNormal.y < 0.99f) {
			float slopeAngle = enhancedController.GroundSlopeAngle;
	
			if (slopeAngle >= slideStartSlopeAngle && slopeAngle <= slideStopSlopeAngle)
				shouldSlide = true;
		}
		return shouldSlide;
	}
}
