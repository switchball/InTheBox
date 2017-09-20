using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(EnhancedCharacterController))]
[RequireComponent (typeof(CharacterMotorForwardWalker))]
public class CharacterMotorPushForce : MonoBehaviour
{
	private class ApplyForceData
	{
		public Rigidbody rigidbody;
		public Vector3 force = Vector3.zero;
		public Vector3 point = Vector3.zero;

		public ApplyForceData (Rigidbody rigidbody, Vector3 force, Vector3 point)
		{
			this.rigidbody = rigidbody;
			this.force = force;
			this.point = point;
		}

		public void Apply ()
		{
			if (rigidbody != null)
				rigidbody.AddForceAtPosition (force, point);
		}
	}

	public List<string> pushableTags = new List<string> (){ "Pushable" };
	public float walkStrengthToForce = 12f;
	public float weightForce = 2f;
	public AnimationCurve weightForceScaleBySlopeAngle = new AnimationCurve (new Keyframe (0, 1), new Keyframe (45, 1), new Keyframe (55, 0));

	private EnhancedCharacterController enhancedController;
	private CharacterMotorForwardWalker walker;
	private List<ApplyForceData> applyForceDatas = new List<ApplyForceData> ();

	void OnEnable ()
	{
		enhancedController = GetComponent<EnhancedCharacterController> ();
		walker = GetComponent<CharacterMotorForwardWalker> ();
		enhancedController.BeforeUpdate += HandleControllerBeforeUpdate;
	}

	void OnDisable ()
	{
		enhancedController.BeforeUpdate -= HandleControllerBeforeUpdate;
	}

	void HandleControllerBeforeUpdate (EnhancedCharacterController enhancedController)
	{
		applyForceDatas.Clear ();
	}

	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (hit.rigidbody != null && IsPushable (hit.rigidbody) && !HasForceDataForRigidbody (hit.rigidbody)) {
			Vector3 force = Vector3.zero;
			if (hit.point.y < enhancedController.BelowSphereCenter.y) {
				force = new Vector3 (0, -weightForce, 0);

				float weightForceScale = 0;
				if (enhancedController.IsGrounded)
					weightForceScale = weightForceScaleBySlopeAngle.Evaluate (enhancedController.GroundSlopeAngle);
				
				force *= weightForceScale;
			} else {
				force = walker.TargetForward * walker.ForwardStrength;
				force *= walkStrengthToForce;
			}
			if (force != Vector3.zero)
				applyForceDatas.Add (new ApplyForceData (hit.rigidbody, force, hit.point));
		}
	}

	void FixedUpdate ()
	{
		if (!enhancedController.enabled)
			applyForceDatas.Clear ();
		
		if (enhancedController.IsGrounded && enhancedController.Ground != null) {
			Rigidbody groundRigidbody = enhancedController.Ground.attachedRigidbody;
			if (groundRigidbody != null && IsPushable (groundRigidbody) && !HasForceDataForRigidbody (groundRigidbody)) {
				Vector3 force = new Vector3 (0, -weightForce, 0);
				float weightForceScale = weightForceScaleBySlopeAngle.Evaluate (enhancedController.GroundSlopeAngle);
				force *= weightForceScale;

				if (force != Vector3.zero)
					applyForceDatas.Add (new ApplyForceData (groundRigidbody, force, enhancedController.BelowCenter));
			}
		}
		applyForceDatas.ForEach (a => a.Apply ());
	}

	private bool HasForceDataForRigidbody (Rigidbody targetRigidbody)
	{
		return applyForceDatas.Find (data => data.rigidbody == targetRigidbody) != null;
	}

	private bool IsPushable (Rigidbody targetRigidbody)
	{
		bool isPushable = false;
		if (!targetRigidbody.isKinematic) {
			foreach (string pushableTag in pushableTags) {
				if (targetRigidbody.CompareTag (pushableTag)) {
					isPushable = true;
					break;
				}
			}
		}
		return isPushable;
	}
}
