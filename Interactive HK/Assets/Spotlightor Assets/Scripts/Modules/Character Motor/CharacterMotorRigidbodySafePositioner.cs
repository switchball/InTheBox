using UnityEngine;
using System.Collections;

[RequireComponent (typeof(EnhancedCharacterController))]
public class CharacterMotorRigidbodySafePositioner : MonoBehaviour
{
	

	public class RigidbodyHelper : MonoBehaviour
	{
		private CollisionFlags lastCollisionFlags = CollisionFlags.Sides;

		public CollisionFlags LastCollisionFlags { get { return lastCollisionFlags; } }

		public bool IsColliding { get; private set; }

		public bool debugMode = false;

		void OnCollisionStay (Collision collision)
		{
			IsColliding = collision.rigidbody != null;
			if (IsColliding) {
				lastCollisionFlags = CollisionFlags.None;
				CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider> ();
				Vector3 centerPos = transform.TransformPoint (capsuleCollider.center);
				foreach (ContactPoint cp in collision.contacts) {
					if (cp.point.y > centerPos.y + capsuleCollider.height * 0.5f)
						lastCollisionFlags = lastCollisionFlags | CollisionFlags.Above;
					else if (cp.point.y < centerPos.y - capsuleCollider.height * 0.5f)
						lastCollisionFlags = lastCollisionFlags | CollisionFlags.Below;
					else
						lastCollisionFlags = lastCollisionFlags | CollisionFlags.Sides;

					Debug.DrawRay (cp.point, cp.normal);
				}
				if (debugMode) {
					string logMsg = string.Format ("Collding with {0} (rigidbody:{1}) ", collision.collider.name, collision.rigidbody != null);
					if ((lastCollisionFlags & CollisionFlags.Above) != 0)
						logMsg += " ABOVE |";
					if ((lastCollisionFlags & CollisionFlags.Below) != 0)
						logMsg += " BELOW |";
					if ((lastCollisionFlags & CollisionFlags.Sides) != 0)
						logMsg += " SIDES |";
				
					this.Log (logMsg);
				}
			}
		}

		void FixedUpdate ()
		{
			IsColliding = false;
		}
	}

	private EnhancedCharacterController enhancedController;
	private CapsuleCollider safePositionerCollider;
	private Rigidbody safePositionerRigidbody;
	private RigidbodyHelper safePositionerHelper;

	public Collider HelperCollider {
		get {
			return safePositionerHelper.GetComponent<Collider> ();
		}
	}

	void OnEnable ()
	{
		enhancedController = GetComponent<EnhancedCharacterController> ();

		CreateSafePositionerObject ();

		enhancedController.Teleported += HandleTeleported;
		enhancedController.BeforeApplyMove += HandleBeforeApplyMove;
	}

	void OnDisable ()
	{
		if (safePositionerRigidbody != null)
			Destroy (safePositionerRigidbody.gameObject);

		enhancedController.Teleported -= HandleTeleported;
		enhancedController.BeforeApplyMove -= HandleBeforeApplyMove;
	}

	private void CreateSafePositionerObject ()
	{
		GameObject safePositioner = new GameObject (gameObject.name + " Rigidbody Safe Positioner");
		safePositioner.layer = gameObject.layer;
		safePositioner.transform.CopyPositionRotation (gameObject.transform);
		safePositioner.transform.localScale = transform.localScale;
		safePositioner.transform.SetSiblingIndex (transform.GetSiblingIndex () + 1);
		
		safePositionerCollider = safePositioner.AddComponent<CapsuleCollider> ();
		SyncColliderShape ();
		
		PhysicMaterial physicMaterial = new PhysicMaterial ("safe_positioner");
		physicMaterial.bounciness = 0;
		physicMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
		physicMaterial.dynamicFriction = 0;
		physicMaterial.staticFriction = 0;
		physicMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
		safePositionerCollider.material = physicMaterial;
		
		safePositionerRigidbody = safePositioner.AddComponent<Rigidbody> ();
		safePositionerRigidbody.isKinematic = false;
		safePositionerRigidbody.useGravity = false;
		safePositionerRigidbody.mass = 0.01f;
		safePositionerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		safePositionerRigidbody.interpolation = RigidbodyInterpolation.Interpolate;

		Physics.IgnoreCollision (safePositionerCollider, enhancedController.Controller);

		safePositionerHelper = safePositioner.AddComponent<RigidbodyHelper> ();
	}

	void HandleTeleported (EnhancedCharacterController enhancedController)
	{
		safePositionerRigidbody.position = enhancedController.transform.position;
	}

	void FixedUpdate ()
	{
		Vector3 deltaPosition = enhancedController.transform.position - safePositionerRigidbody.transform.position;

		if (deltaPosition.magnitude > enhancedController.Radius * 2 || Time.fixedDeltaTime < 0.001f)
			safePositionerRigidbody.position += deltaPosition;
		else
			safePositionerRigidbody.velocity = deltaPosition / Time.fixedDeltaTime;

		SyncColliderShape ();
	}

	void HandleBeforeApplyMove (EnhancedCharacterController enhancedController, Vector3 totalMotion)
	{
		if (safePositionerHelper.IsColliding) {
			Vector3 pushMovement = safePositionerRigidbody.transform.position - transform.position;

			if ((safePositionerHelper.LastCollisionFlags & CollisionFlags.Sides) == 0)
				pushMovement.x = pushMovement.z = 0;
			if ((safePositionerHelper.LastCollisionFlags & CollisionFlags.Above) == 0
			    && (safePositionerHelper.LastCollisionFlags & CollisionFlags.Below) == 0)
				pushMovement.y = 0;

//			ApplyPushMovementByTransform (pushMovement);
			ApplyPushMovementByControllerMove (pushMovement); // CC move up lost ground collision has been solved, so transform movement is no longer needed.
//			ApplyPushMovementHybrid (pushMovement);
		}
	}

	// Method 1: Believe in safePositionerRigidbody's position must be valid, move transform will be ok(Not breaking the physics world).
	// Cons: CC may sink into ground by Chains sometimes
//	private void ApplyPushMovementByTransform (Vector3 pushMovement)
//	{
//		transform.position += pushMovement;
//	}

	// Method 2: Afarid of breaking physics world, move by Controller.Move.
	// Cons: Climbing up push book
	private void ApplyPushMovementByControllerMove (Vector3 pushMovement)
	{
		enhancedController.Move (pushMovement);
	}

	// Method 3: 1 + 2, won't sink into ground nor run up moving rigidbody.
//	private void ApplyPushMovementHybrid (Vector3 pushMovement)
//	{
//		// Apply vertical movement by Controller. CC.Move to not sink to ground if push from top
//		enhancedController.Move (new Vector3 (0, pushMovement.y, 0)); 
//
//		transform.position += new Vector3 (pushMovement.x, 0, pushMovement.z);
//	}

	private void SyncColliderShape ()
	{
		if (safePositionerCollider.center != enhancedController.Controller.center)
			safePositionerCollider.center = enhancedController.Controller.center;
		
		if (safePositionerCollider.height != enhancedController.Controller.height)
			safePositionerCollider.height = enhancedController.Controller.height;
		
		if (safePositionerCollider.radius != enhancedController.Controller.radius)
			safePositionerCollider.radius = enhancedController.Controller.radius;
	}
}
