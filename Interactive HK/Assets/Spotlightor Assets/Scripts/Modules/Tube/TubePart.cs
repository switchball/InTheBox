using UnityEngine;
using System.Collections;

public class TubePart : MonoBehaviour
{
	public Vector3 endPosition;
	public Vector3 endRotation;

	public Vector3 EndWorldPosition{ get { return transform.TransformPoint (endPosition); } }

	public Quaternion EndWorldRotation{ get { return transform.rotation * Quaternion.Euler (endRotation); } }

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.white;
		float pointRadius = 0.2f;
		Gizmos.DrawSphere (transform.TransformPoint (endPosition), pointRadius);
		Gizmos.DrawRay (transform.TransformPoint (endPosition), Quaternion.Euler (endRotation) * transform.forward);
	}
}
