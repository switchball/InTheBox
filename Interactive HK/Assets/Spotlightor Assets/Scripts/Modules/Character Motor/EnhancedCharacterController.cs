using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(CharacterController))]
public class EnhancedCharacterController : MonoBehaviour
{
	private const string MessageGroundEnter = "OnGroundEnter";
	private const string MessageGroundExit = "OnGroundExit";

	public delegate void GenericEventHandler (EnhancedCharacterController enhancedController);

	public delegate void GroundEventHandler (EnhancedCharacterController enhancedController,Collider ground);

	public delegate void MoveEventHandler (EnhancedCharacterController enhancedController,Vector3 totalMotion);

	public event GroundEventHandler Grounded;
	public event GroundEventHandler Floated;
	public event GroundEventHandler GroundEntered;
	public event GroundEventHandler GroundExited;
	public event GenericEventHandler BeforeUpdate;
	public event GenericEventHandler Teleported;
	public event MoveEventHandler BeforeApplyMove;
	public event MoveEventHandler AfterApplyMove;

	// To remove "offset" effect of moving platform (due to platform updated after this.Update)
	[Header ("Script Order: 1 (After DefaultTime)")]
	public float gravity = -20;
	public LayerMask nearGroundLayerMask = -1;
	[ClampAttribute (0.001f, float.MaxValue)]
	public float nearGroundDistance = 0.1f;
	#if UNITY_4
	public float skinWidth = 0.1f;
	#endif
	private Collider ground;
	private Vector3 totalMotion = Vector3.zero;
	private Vector3 totalAcceleration = Vector3.zero;
	private Vector3 speed = Vector3.zero;
	private Vector3 forward = Vector3.forward;
	private Vector3 velocity = Vector3.zero;

	public float SkinWidth {
		#if UNITY_4
		get {return skinWidth;}
		#else
		get { return Controller.skinWidth; }
		#endif
	}

	public Vector3 UpCenter { get { return Center + Vector3.up * Height * 0.5f; } }

	public Vector3 BelowCenter { get { return Center + Vector3.down * Height * 0.5f; } }

	public Vector3 UpSphereCenter{ get { return Center + Vector3.up * (Controller.height * 0.5f - Controller.radius); } }

	public Vector3 BelowSphereCenter{ get { return Center + Vector3.down * (Controller.height * 0.5f - Controller.radius); } }

	public Vector3 Center{ get { return transform.position + Controller.center; } }

	public float Radius{ get { return Controller.radius + SkinWidth; } }

	public float Height{ get { return Controller.height + 2 * SkinWidth; } }

	public CharacterController Controller { get { return GetComponent<CharacterController> (); } }

	public bool IsGrounded {
		get {
			bool isGrounded = Controller.isGrounded;
			if (!isGrounded && IsNearGrounded)// Why do we need this test? Because of unreliable near ground?
				isGrounded = Vector3.Distance (GroundHitPoint, BelowSphereCenter) <= Radius;

			return isGrounded;
		}
	}

	public bool IsNearGrounded{ get { return Ground != null; } }

	public Collider Ground { 
		get{ return ground; }
		private set {
			if (ground != value) {
				if (ground != null) {
					if (GroundExited != null)
						GroundExited (this, ground);
					SendMessageToColliderAndRigidbody (ground, MessageGroundExit, this);
				}

				Collider lastGround = ground;
				ground = value;
				
				if (ground != null) {
					if (GroundEntered != null)
						GroundEntered (this, ground);
					SendMessageToColliderAndRigidbody (ground, MessageGroundEnter, this);

					if (lastGround == null) {
						if (Grounded != null)
							Grounded (this, ground);
					}
				} else {
					LastLeaveGroundTime = Time.time;

					if (Floated != null)
						Floated (this, lastGround);
				}
			}
		} 
	}

	public float LastLeaveGroundTime{ get; private set; }

	public Vector3 GroundHitPoint{ get; private set; }

	public float GroundSlopeAngle {
		get {
			float groundHitNormalXZLength = new Vector3 (GroundHitNormal.x, 0, GroundHitNormal.z).magnitude;
			return 90 - Mathf.Atan2 (GroundHitNormal.y, groundHitNormalXZLength) * Mathf.Rad2Deg;
		}
	}

	public Vector3 GroundHitNormal{ get; private set; }

	public Vector3 GroundHitMovement{ get; private set; }

	public Vector3 Speed {
		get { return speed; }
		set { speed = value; }
	}

	public float SpeedX {
		get { return speed.x; }
		set { speed.x = value; }
	}

	public float SpeedY {
		get { return speed.y; }
		set { speed.y = value; }
	}

	public float SpeedZ {
		get { return speed.z; }
		set { speed.z = value; }
	}

	public Vector3 Forward {
		get { return forward; }
		set {
			value.y = 0;
			if (value != Vector3.zero)
				forward = value.normalized;
		}
	}

	public Vector3 Velocity { get { return velocity; } }

	public CollisionFlags TotalCollisionFlags{ get; private set; }

	private void SendMessageToColliderAndRigidbody (Collider groundCollider, string message, object data)
	{
		groundCollider.SendMessage (message, data, SendMessageOptions.DontRequireReceiver);
		if (groundCollider.attachedRigidbody != null && groundCollider.attachedRigidbody.gameObject != groundCollider.gameObject)
			groundCollider.attachedRigidbody.SendMessage (message, data, SendMessageOptions.DontRequireReceiver);
	}

	public void Move (Vector3 motion)
	{
		totalMotion += motion;
	}

	public void Accelerate (Vector3 acceleration)
	{
		totalAcceleration += acceleration;
	}

	public void TeleportTo (Vector3 position)
	{
		transform.position = position;
		this.Speed = Vector3.zero;
		this.Ground = null;

		if (Teleported != null)
			Teleported (this);
	}

	private void Update ()
	{
		Vector3 positionBeforeUpdate = transform.position;

		if (BeforeUpdate != null)
			BeforeUpdate (this);

		totalAcceleration.y += gravity;

		speed += totalAcceleration * Time.deltaTime;
		totalMotion += speed * Time.deltaTime;

		if (BeforeApplyMove != null)
			BeforeApplyMove (this, totalMotion);

		transform.forward = Forward;

		TotalCollisionFlags = CollisionFlags.None;
		if (Controller.enabled) {
			if (totalMotion.y < 0)
				TotalCollisionFlags |= Controller.Move (totalMotion);
			else {
				float bonusY = SkinWidth + 0.01f;
				TotalCollisionFlags |= Controller.Move (totalMotion + Vector3.up * bonusY);

				if ((TotalCollisionFlags & CollisionFlags.Above) != 0)
					SpeedY = Mathf.Min (0, SpeedY);
				
				TotalCollisionFlags |= Controller.Move (Vector3.down * bonusY);
			}
		}

		if (Controller.isGrounded == false)
			DetectIsNearGroundedByRaycast ();

//		if(GroundSlopeAngle > 70 && Controller.isGrounded)
//			this.Log ("Ground slope angle > 70. Slope angle = {0}, speedY = {1}, ground= {2}", GroundSlopeAngle, SpeedY, Ground);

//		if(GroundSlopeAngle > 70 && IsNearGrounded)
//			this.Log ("Ground slope angle > 70 && nearGrounded. Slope angle = {0}, speedY = {1}, nearground = {2}, Ground ={3}", 
//				GroundSlopeAngle, SpeedY, IsNearGrounded, Ground);

		if (IsGrounded && SpeedY < 0) {
			// Land on ground and stop falling
			if (GroundSlopeAngle < 73)// Magic number?
				SpeedY = 0;
			else {
				// No a flat ground, we won't stop SpeedY because we want to make it continue falling.
				// but we are not sure if it can falling (88 is also considered grounded, and stuck)
				if (SpeedY < Controller.velocity.y && Controller.velocity.y <= 0)
					SpeedY = Controller.velocity.y;
			}
		}

		if (AfterApplyMove != null)
			AfterApplyMove (this, totalMotion);

		totalMotion = Vector3.zero;
		totalAcceleration = Vector3.zero;

		Vector3 movementThisFrame = transform.position - positionBeforeUpdate;
		if (Time.timeScale > 0)
			velocity = movementThisFrame / Time.deltaTime;
	}

	private void DetectIsNearGroundedByRaycast ()
	{
		// According to tests with CharacterController (especially isGrounded on slope wall), collide inside side-skin-width is not considered as ground collide.
		// So we should use Controller.radius to make raycast hit test.
		float castRadius = Controller.radius;
		float castDistance = Controller.radius + SkinWidth + nearGroundDistance;

		RaycastHit[] groundHits = Physics.SphereCastAll (BelowSphereCenter + Vector3.up * castRadius, castRadius, Vector3.down, castDistance, nearGroundLayerMask);
		if (groundHits != null && groundHits.Length > 0) {
			float minDistanceToBelow = float.MaxValue;
			int bestGroundHitIndex = -1;
			for (int i = 0; i < groundHits.Length; i++) {
				RaycastHit hit = groundHits [i];
				Vector3 hitOffsetToCenter = hit.point - Center;
				hitOffsetToCenter.y = 0;
				float distanceToCenter = hitOffsetToCenter.magnitude;
				if (!hit.collider.isTrigger && hit.collider != Controller
				    && hit.point.y < BelowSphereCenter.y
				    && distanceToCenter < castRadius) {
					float distanceToBelow = (hit.point - BelowCenter).magnitude;
					if (distanceToBelow < minDistanceToBelow) {
						minDistanceToBelow = distanceToCenter;
						bestGroundHitIndex = i;
					}
				}
				Debug.DrawRay (hit.point, hit.normal, Color.yellow);
			}
			if (bestGroundHitIndex >= 0) {
				RaycastHit bestGroundHit = groundHits [bestGroundHitIndex];
				GroundHitPoint = bestGroundHit.point;
				GroundHitNormal = bestGroundHit.normal;
				GroundHitMovement = totalMotion;
				Ground = bestGroundHit.collider;
			} else
				Ground = null;
		} else
			Ground = null;
	}

	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (hit.point.y < BelowSphereCenter.y) {
			bool updateGround = false;
			if (this.Ground == null)
				updateGround = true;
			else {
				if (this.Ground == hit.collider)
					updateGround = true;
				else {
//					if (Vector3.Distance (this.GroundHitPoint, BelowCenter) > Vector3.Distance (hit.point, BelowCenter))
					updateGround = true;
				}
			}
			if (updateGround) {
				GroundHitPoint = hit.point;
				GroundHitNormal = hit.normal;
				if (hit.normal.y < 0)
					GroundHitNormal = -hit.normal;// Sphere collider ground hit.normal is pointing sphere's center!? Flip it. Reproduce this "bug" in Scene Physics(Slope)
				GroundHitMovement = hit.moveDirection * hit.moveLength;
				Ground = hit.collider;

				Debug.DrawRay (hit.point, hit.normal, Color.green);
			}
		}
	}

	void OnDisable ()
	{
		Ground = null;
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine (BelowCenter, BelowCenter + Vector3.up * SkinWidth);
		Gizmos.DrawWireSphere (BelowSphereCenter, Controller.radius + SkinWidth);

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine (BelowCenter, BelowCenter + Vector3.down * nearGroundDistance);
		Gizmos.DrawWireSphere (BelowSphereCenter + Vector3.down * (nearGroundDistance + SkinWidth), Controller.radius);
	}

	void OnDrawGizmos ()
	{
		if (Application.isPlaying) {
			if (IsNearGrounded) {
				Gizmos.color = Color.red;
				Gizmos.DrawSphere (GroundHitPoint, 0.05f);
				Gizmos.DrawRay (GroundHitPoint, GroundHitNormal);
			}
		}
	}
}
