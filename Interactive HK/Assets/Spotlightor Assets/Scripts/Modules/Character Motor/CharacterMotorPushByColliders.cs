using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnhancedCharacterController))]
public class CharacterMotorPushByColliders : FunctionalMonoBehaviour
{
	// Execute before CharacterMotorMovingPlatformer, to make platformer saved the final tranform position & rotation.
	[Header("Script Order: 2")]
	public LayerMask
		pusherLayerMask = -1;
	public bool offsetAsTransformMovement = true;
	private EnhancedCharacterController enhancedController;

	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		if (forTheFirstTime)
			enhancedController = GetComponent<EnhancedCharacterController> ();

		enhancedController.AfterApplyMove += HandleAfterApplyMove;
		enhancedController.BeforeApplyMove += HandleBeforeApplyMove;
	}

	protected override void OnBecameUnFunctional ()
	{
		enhancedController.AfterApplyMove -= HandleAfterApplyMove;
		enhancedController.BeforeApplyMove -= HandleBeforeApplyMove;
	}
	
	void HandleBeforeApplyMove (EnhancedCharacterController enhancedController, Vector3 totalMotion)
	{
//		if (!offsetAsTransformMovement)
//			PushByColliders ();
	}

	void HandleAfterApplyMove (EnhancedCharacterController enhancedController, Vector3 totalMotion)
	{
//		if (offsetAsTransformMovement)
			PushByColliders ();
	}

	void PushByColliders ()
	{
		float radius = enhancedController.Controller.radius;
		float height = enhancedController.Controller.height;
		Vector3 upSphereCenter = enhancedController.UpSphereCenter;
		Vector3 belowSphereCenter = enhancedController.BelowSphereCenter;

		RaycastHit hit = new RaycastHit ();
		bool hitCollider = false;
		// Down
		if (!hitCollider) {
			Vector3 downCastOrigin = upSphereCenter + Vector3.up * 2 * radius;
			float downCastDistance = height + enhancedController.SkinWidth;
			hitCollider = Physics.SphereCast (downCastOrigin, radius, Vector3.down, out hit, downCastDistance, pusherLayerMask);
			if (hitCollider && hit.collider.isTrigger)
				hitCollider = false;
			if (hitCollider) {
				if (hit.point.y > upSphereCenter.y + radius)
					hitCollider = false;
				else if (hit.point.y > upSphereCenter.y) {
					if (Vector3.Distance (hit.point, upSphereCenter) > radius) {
						hitCollider = false;
					}
				}
			}
		}
		if (hitCollider) 
			GotoSafePlace (hit, Vector3.down);
		hitCollider = false;
		
		if (Physics.CheckCapsule (upSphereCenter, belowSphereCenter, radius, pusherLayerMask)) {
//			this.Log ("Colliders in CharacterController! Check the other axises: up, forward, backward, left, right");

			// Up
			if (!hitCollider) {
				Vector3 upCastOrigin = belowSphereCenter + Vector3.down * 2 * radius;
				float upCastDistance = height;
				hitCollider = Physics.SphereCast (upCastOrigin, radius, Vector3.up, out hit, upCastDistance, pusherLayerMask);
				if (hitCollider && hit.collider.isTrigger)
					hitCollider = false;
				if (hitCollider) {
					if (hit.point.y < belowSphereCenter.y - radius)
						hitCollider = false;
					else if (hit.point.y < belowSphereCenter.y) {
						if (Vector3.Distance (hit.point, belowSphereCenter) > radius) {
							hitCollider = false;
						}
					}
				}

			}
			if (hitCollider) 
				GotoSafePlace (hit, Vector3.up);
			hitCollider = false;

			// Backward
			if (!hitCollider) {
				Vector3 backwardCastPoint1 = upSphereCenter + Vector3.forward * 2 * radius;
				Vector3 backwardCastPoint2 = belowSphereCenter + Vector3.forward * 2 * radius;
				float forwardCastDistance = 2 * radius;
				hitCollider = Physics.CapsuleCast (backwardCastPoint1, backwardCastPoint2, radius, Vector3.back, out hit, forwardCastDistance, pusherLayerMask);
				if (hitCollider && hit.collider.isTrigger)
					hitCollider = false;
				if (hitCollider) {
					if (hit.point.z > upSphereCenter.z + radius)
						hitCollider = false;
					else if (hit.point.z > upSphereCenter.z) {
						if (hit.point.y > upSphereCenter.y) {
							if (Vector3.Distance (hit.point, upSphereCenter) > radius)
								hitCollider = false;
						} else if (hit.point.y < belowSphereCenter.y) {
							if (Vector3.Distance (hit.point, belowSphereCenter) > radius)
								hitCollider = false;
						} else {
							Vector3 centerOffset = hit.point - upSphereCenter;
							centerOffset.y = 0;
							if (centerOffset.magnitude > radius)
								hitCollider = false;
						}
					}
				}
			}
			if (hitCollider) 
				GotoSafePlace (hit, Vector3.back);
			hitCollider = false;

			// Forward
			if (!hitCollider) {
				Vector3 forwardCastPoint1 = upSphereCenter + Vector3.back * 2 * radius;
				Vector3 forwardCastPoint2 = belowSphereCenter + Vector3.back * 2 * radius;
				float forwardCastDistance = 2 * radius;
				hitCollider = Physics.CapsuleCast (forwardCastPoint1, forwardCastPoint2, radius, Vector3.forward, out hit, forwardCastDistance, pusherLayerMask);
				if (hitCollider && hit.collider.isTrigger)
					hitCollider = false;
				if (hitCollider) {
					if (hit.point.z < upSphereCenter.z - radius)
						hitCollider = false;
					else if (hit.point.z < upSphereCenter.z) {
						if (hit.point.y > upSphereCenter.y) {
							if (Vector3.Distance (hit.point, upSphereCenter) > radius)
								hitCollider = false;
						} else if (hit.point.y < belowSphereCenter.y) {
							if (Vector3.Distance (hit.point, belowSphereCenter) > radius)
								hitCollider = false;
						} else {
							Vector3 centerOffset = hit.point - upSphereCenter;
							centerOffset.y = 0;
							if (centerOffset.magnitude > radius)
								hitCollider = false;
						}
					}
				}
			}
			if (hitCollider) 
				GotoSafePlace (hit, Vector3.forward);
			hitCollider = false;

			// Right
			if (!hitCollider) {
				Vector3 leftCastPoint1 = upSphereCenter + Vector3.left * 2 * radius;
				Vector3 rightCastPoint2 = belowSphereCenter + Vector3.left * 2 * radius;
				float rightCastDistance = 2 * radius;
				hitCollider = Physics.CapsuleCast (leftCastPoint1, rightCastPoint2, radius, Vector3.right, out hit, rightCastDistance, pusherLayerMask);
				if (hitCollider && hit.collider.isTrigger)
					hitCollider = false;
				if (hitCollider) {
					if (hit.point.x < upSphereCenter.x - radius)
						hitCollider = false;
					else if (hit.point.x < upSphereCenter.x) {
						if (hit.point.y > upSphereCenter.y) {
							if (Vector3.Distance (hit.point, upSphereCenter) > radius)
								hitCollider = false;
						} else if (hit.point.y < belowSphereCenter.y) {
							if (Vector3.Distance (hit.point, belowSphereCenter) > radius)
								hitCollider = false;
						} else {
							Vector3 centerOffset = hit.point - upSphereCenter;
							centerOffset.y = 0;
							if (centerOffset.magnitude > radius)
								hitCollider = false;
						}
					}
				}
			}
			if (hitCollider) 
				GotoSafePlace (hit, Vector3.right);
			hitCollider = false;

			// Left
			if (!hitCollider) {
				Vector3 leftCastPoint1 = upSphereCenter + Vector3.right * 2 * radius;
				Vector3 leftCastPoint2 = belowSphereCenter + Vector3.right * 2 * radius;
				float rightCastDistance = 2 * radius;
				hitCollider = Physics.CapsuleCast (leftCastPoint1, leftCastPoint2, radius, Vector3.left, out hit, rightCastDistance, pusherLayerMask);
				if (hitCollider && hit.collider.isTrigger)
					hitCollider = false;
				if (hitCollider) {
					if (hit.point.x > upSphereCenter.x + radius)
						hitCollider = false;
					else if (hit.point.x > upSphereCenter.x) {
						if (hit.point.y > upSphereCenter.y) {
							if (Vector3.Distance (hit.point, upSphereCenter) > radius)
								hitCollider = false;
						} else if (hit.point.y < belowSphereCenter.y) {
							if (Vector3.Distance (hit.point, belowSphereCenter) > radius)
								hitCollider = false;
						} else {
							Vector3 centerOffset = hit.point - upSphereCenter;
							centerOffset.y = 0;
							if (centerOffset.magnitude > radius)
								hitCollider = false;
						}
					}
				}
			}
			if (hitCollider) 
				GotoSafePlace (hit, Vector3.left);
			hitCollider = false;

		}
	}

	private void GotoSafePlace (RaycastHit hit, Vector3 checkDir)
	{
		Vector3 toSafeoffset = Vector3.zero;
		Vector3 bounceCenter = Vector3.zero;
		if (hit.point.y > enhancedController.UpSphereCenter.y) {
			bounceCenter = enhancedController.UpSphereCenter;
		} else if (hit.point.y < enhancedController.BelowSphereCenter.y) {
			bounceCenter = enhancedController.BelowSphereCenter;
		} else {
			bounceCenter = enhancedController.Center;
			bounceCenter.y = hit.point.y;
		}

		Vector3 hitToCenterHorizontalOffset = hit.point - enhancedController.Center;
		hitToCenterHorizontalOffset.y = 0;
		bool isFootHit = hit.point.y < enhancedController.BelowSphereCenter.y && hitToCenterHorizontalOffset.magnitude < 0.01f;
		if (Vector3.Distance (hit.point, bounceCenter) <= enhancedController.Controller.radius
			|| isFootHit) {
			float safeRadius = isFootHit ? enhancedController.Radius : enhancedController.Controller.radius;
			Vector3 hitToCenterDir = (hit.point - bounceCenter).normalized;
			bool isThickHit = Vector3.Angle (hitToCenterDir, checkDir) > 90;// Axis of CharacterController has sinked into the collider.
			Vector3 safePoint = bounceCenter + (isThickHit ? -1 : 1) * hitToCenterDir * safeRadius;
			toSafeoffset = hit.point - safePoint;

			if (offsetAsTransformMovement)
				transform.position += toSafeoffset;
			else
				enhancedController.Move (toSafeoffset);
		}

	}
}
