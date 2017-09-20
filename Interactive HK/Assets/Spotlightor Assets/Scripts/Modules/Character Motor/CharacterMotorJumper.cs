using UnityEngine;
using System.Collections;

[RequireComponent (typeof(EnhancedCharacterController))]
public class CharacterMotorJumper : MonoBehaviour
{
	public delegate void GenericEventHandler (CharacterMotorJumper jumper);

	public event GenericEventHandler JumpStarted;
	public event GenericEventHandler JumpFloatingEnded;
	public event GenericEventHandler JumpEnded;

	public float minJumpHeight = 1.2f;
	public float maxJumpHeight = 2.4f;
	public float slopeLimitForJump = 60;
	public float floatForceDelay = 0.1f;
	public float airJumpDelay = 0.1f;
	private bool isJumping = false;
	private bool isHoldingJump = false;
	private bool isJumpFloating = false;
	private float jumpTimeElasped = 0;
	private bool hasUpdated = false;

	public bool IsJumping {
		get { return isJumping; }
		set {
			if (isJumping != value) {
				isJumping = value;
				if (isJumping) {
					if (JumpStarted != null)
						JumpStarted (this);
				} else {
					if (JumpEnded != null)
						JumpEnded (this);
				}
			}
		}
	}

	public bool IsHoldingJump {
		get { return isHoldingJump; }
		set {
			if (value != isHoldingJump) {
				isHoldingJump = value;
				if (!isHoldingJump && IsJumpFloating)
					IsJumpFloating = false;
			}
		}
	}

	public bool IsJumpFloating {
		get { return isJumpFloating; }
		set {
			if (value != isJumpFloating) {
				isJumpFloating = value;
				if (!isJumpFloating) {
					if (JumpFloatingEnded != null)
						JumpFloatingEnded (this);
				}
			}
		}
	}

	private float TimeToReachMaxJumpHeight {
		get { 
//			return Mathf.Sqrt (2 * -maxJumpHeight / (enhancedController.gravity + JumpFloatAcceleration)); 
			float jumpSpeedForMinHeight = this.JumpStartSpeedForMinHeight;
			float floatJumpStartSpeed = jumpSpeedForMinHeight + enhancedController.gravity * floatForceDelay;
			return floatForceDelay - floatJumpStartSpeed / (enhancedController.gravity + JumpFloatAcceleration);
		}
	}

	public float JumpFloatAcceleration {
		get {
//			float jumpSpeedForMinHeight = this.JumpStartSpeedForMinHeight;
//			return (-enhancedController.gravity - 0.5f * jumpSpeedForMinHeight * jumpSpeedForMinHeight / maxJumpHeight);
			float jumpSpeedForMinHeight = this.JumpStartSpeedForMinHeight;
			float minHeightJumpSpeedDistance = jumpSpeedForMinHeight * floatForceDelay + 0.5f * enhancedController.gravity * floatForceDelay * floatForceDelay;
			float floatJumpDistance = maxJumpHeight - minHeightJumpSpeedDistance;
			float floatJumpStartSpeed = jumpSpeedForMinHeight + enhancedController.gravity * floatForceDelay;
			return -enhancedController.gravity - 0.5f * floatJumpStartSpeed * floatJumpStartSpeed / floatJumpDistance;
		}
	}

	private float TimeToReachMinHeight {
		get { return Mathf.Sqrt (2 * -minJumpHeight / enhancedController.gravity); }
	}

	private float JumpStartSpeedForMinHeight {
		get { return Mathf.Sqrt (2 * -minJumpHeight * enhancedController.gravity); }
	}

	private EnhancedCharacterController enhancedController;

	void Reset ()
	{
		if (GetComponent<CharacterController> () != null)
			slopeLimitForJump = GetComponent<CharacterController> ().slopeLimit;
	}


	void Start ()
	{
		enhancedController = GetComponent<EnhancedCharacterController> ();
	}

	// Before EnhancedController.Update
	private void Update ()
	{
		if (IsJumping
		    && enhancedController.SpeedY <= 0.0f
		    && enhancedController.IsNearGrounded) {// Stop jump once nearGrounded, allow player making another jump more easily.) 
			IsJumping = false;
		}

		if (IsJumping && IsJumpFloating) {
			if ((enhancedController.Controller.collisionFlags & CollisionFlags.Above) != 0)
				IsJumpFloating = false;
		}

		if (IsJumping && IsJumpFloating) {
			float a = enhancedController.gravity;
			bool isAccelerating = jumpTimeElasped >= floatForceDelay;
			if(isAccelerating){
				a += JumpFloatAcceleration;
			}

			float idealJumpHeight = enhancedController.SpeedY * Time.deltaTime + 0.5f * a * Time.deltaTime * Time.deltaTime;
			if(isAccelerating)
				enhancedController.SpeedY += JumpFloatAcceleration * Time.deltaTime;

			float updateJumpHeight = (enhancedController.SpeedY + enhancedController.gravity * Time.deltaTime)*Time.deltaTime;
			enhancedController.Move (Vector3.up * (idealJumpHeight - updateJumpHeight));

			jumpTimeElasped += Time.deltaTime;

			if (jumpTimeElasped > TimeToReachMaxJumpHeight)
				IsJumpFloating = false;
		}

		hasUpdated = true;
	}

	private void LateUpdate ()
	{
		hasUpdated = false;
	}

	public void Jump ()
	{
		if (minJumpHeight > 0 && maxJumpHeight > 0 && !IsJumping) {
			bool jumpable = false;
			// Jumpable judged by nearGrounded, make sure player could jump even floating on some little gaps above grounds.
			if (enhancedController.IsNearGrounded)
				jumpable = enhancedController.GroundSlopeAngle <= slopeLimitForJump;
			else
				jumpable = (Time.time - enhancedController.LastLeaveGroundTime) < airJumpDelay;
			
			if (jumpable) {
				// Instant jump up even when falling down. Good for responsive 2nd jump
				enhancedController.SpeedY = Mathf.Max (0, enhancedController.SpeedY);

				enhancedController.SpeedY += JumpStartSpeedForMinHeight;
				if (hasUpdated && floatForceDelay > 0) {
					// Update() has been called this frame, so FloatAcceleration won't be applied in this frame's EnhancedController.Update().
					// This'll result in a less jump height than target value. (The lower the FPS, the less jump height)
					// So add the single missing FloatAcceleration directly in Jump().
					enhancedController.SpeedY += JumpFloatAcceleration * Time.deltaTime;
				}

				IsHoldingJump = true;
				IsJumpFloating = true;
				IsJumping = true;

				jumpTimeElasped = 0;
			}
		} 
	}
}
