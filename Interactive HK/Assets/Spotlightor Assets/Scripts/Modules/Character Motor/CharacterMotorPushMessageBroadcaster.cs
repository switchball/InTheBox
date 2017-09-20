using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(EnhancedCharacterController))]
[RequireComponent (typeof(CharacterMotorForwardWalker))]
// TODO: Move to CharacterMotorPushForce
public class CharacterMotorPushMessageBroadcaster : MonoBehaviour
{
	public struct PushInfo
	{
		public Vector3 force;
		public Vector3 point;
		public GameObject gameObject;
	}

	public List<string>
		pushableTags = new List<string> (){ "Pushable" };
	public float walkStrengthToForce = 1f;
	public float weightForce = 2f;
	private EnhancedCharacterController enhancedController;
	private CharacterMotorForwardWalker walker;

	void Start ()
	{
		enhancedController = GetComponent<EnhancedCharacterController> ();
		walker = GetComponent<CharacterMotorForwardWalker> ();
	}

	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (hit.rigidbody != null && IsPushable (hit.rigidbody)) {
			PushInfo pushInfo = new PushInfo ();
			if (hit.point.y < enhancedController.BelowSphereCenter.y) {
				pushInfo.force = new Vector3 (0, -weightForce, 0);
			} else {
				pushInfo.force = walker.TargetForward * walker.ForwardStrength;
				pushInfo.force *= walkStrengthToForce;
			}
			pushInfo.point = hit.point;
			pushInfo.gameObject = gameObject;

			hit.rigidbody.SendMessage ("OnPushByCharacterMotor", pushInfo, SendMessageOptions.DontRequireReceiver);
		}
	}

	private bool IsPushable (Rigidbody targetRigidbody)
	{
		bool isPushable = false;
		foreach (string pushableTag in pushableTags) {
			if (targetRigidbody.CompareTag (pushableTag)) {
				isPushable = true;
				break;
			}
		}
		return isPushable;
	}
}
