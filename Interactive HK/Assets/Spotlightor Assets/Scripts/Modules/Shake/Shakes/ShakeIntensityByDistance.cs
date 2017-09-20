using UnityEngine;
using System.Collections;

public class ShakeIntensityByDistance : ShakeDecorator<Shake>
{
	public Transform distanceTarget;
	public float minDistance = 1;
	public float maxDistance = 10;
	public AnimationCurve intensityFallOff = new AnimationCurve (new Keyframe (0, 1), new Keyframe (1, 0));
	
	protected override Vector3 GetCurrentIntensity ()
	{
		Vector3 intensity = shake.Intensity;
		if (distanceTarget != null) {
			float distance = Vector3.Distance (transform.position, distanceTarget.position);
			if (distance > maxDistance)
				intensity = Vector3.zero;
			else if (distance > minDistance) {
				float fallOffT = Mathf.InverseLerp (minDistance, maxDistance, distance);
				intensity *= intensityFallOff.Evaluate (fallOffT);
			}
		} else
			this.Log ("distanceTarget == null, shake intensity will not be scaled.");
		return intensity;
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, minDistance);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, maxDistance);
	}
}
