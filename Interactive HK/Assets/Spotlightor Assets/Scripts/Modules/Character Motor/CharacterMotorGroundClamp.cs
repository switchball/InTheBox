using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnhancedCharacterController))]
public class CharacterMotorGroundClamp : FunctionalMonoBehaviour
{
	// Execute at last, to do the final clamp.
	[Header("Script Order: 5")]
	public float
		clampSlopeLimit = 60;
	private EnhancedCharacterController enhancedController;

	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		if (forTheFirstTime)
			enhancedController = GetComponent<EnhancedCharacterController> ();

		enhancedController.BeforeApplyMove += HandleBeforeApplyMove;
	}

	protected override void OnBecameUnFunctional ()
	{
		enhancedController.BeforeApplyMove -= HandleBeforeApplyMove;
	}

	void HandleBeforeApplyMove (EnhancedCharacterController source, Vector3 totalMotion)
	{
		if (enhancedController.IsNearGrounded 
			&& enhancedController.Speed.y <= 0 
			&& enhancedController.GroundHitNormal.y < 0.99f
			&& enhancedController.GroundSlopeAngle <= clampSlopeLimit) {
			ClampByRaycastPredict (totalMotion);
		}
	}
	
	private void ClampByRaycastPredict (Vector3 totalMotion)
	{
		Vector3 predictOrigin = enhancedController.GroundHitPoint + totalMotion + Vector3.up * enhancedController.SkinWidth;

		Vector3 horizontalMotion = new Vector3 (totalMotion.x, 0, totalMotion.z);
		float castDistance = Mathf.Tan (clampSlopeLimit * Mathf.Deg2Rad) * horizontalMotion.magnitude;
		castDistance += enhancedController.nearGroundDistance + enhancedController.SkinWidth;

		RaycastHit clampHit;
		if (Physics.Raycast (predictOrigin, Vector3.down, out clampHit, castDistance, enhancedController.nearGroundLayerMask)) {
			float deltaY = clampHit.point.y - predictOrigin.y;
			
			enhancedController.Move (Vector3.up * deltaY);
		}
	}
}
