using UnityEngine;
using System.Collections;

public class MoveTransitionManager : ValueTransitionManager
{
	public Vector3 posIn = Vector3.one;
	public Vector3 posOut = Vector3.zero;
	public bool isLocal = true;

	public virtual Vector3 PositionOut { get { return posOut; } }

	protected override void OnProgressValueUpdated (float progress)
	{
		Vector3 pos = MathAddons.LerpUncapped (PositionOut, posIn, progress);
		if (isLocal && transform.parent != null)
			pos = transform.parent.TransformPoint (pos);

		Rigidbody myRigidbody = GetComponent<Rigidbody> ();
		if (myRigidbody != null)
			myRigidbody.MovePosition (pos);
		else
			transform.position = pos;
	}

	void Reset ()
	{
		posIn = posOut = transform.localPosition;
	}

	void OnDrawGizmosSelected ()
	{
		Bounds bounds = new Bounds (transform.position, Vector3.one);

		Collider collider = GetComponent<Collider> ();
		if (collider != null)
			bounds = collider.bounds;
		else {
			Renderer rd = GetComponentInChildren<Renderer> ();
			if (rd != null)
				bounds = rd.bounds;
		}

		Vector3 currentLocalPos = transform.localPosition;
		Gizmos.color = new Color (0, 0, 1, 0.3f);
		Vector3 posInBoundsOffset = posIn - currentLocalPos;
		if (transform.parent != null)
			posInBoundsOffset = transform.parent.TransformVector (posInBoundsOffset);
		Gizmos.DrawWireCube (bounds.center + posInBoundsOffset, bounds.size);

		Gizmos.color = new Color (1, 0, 0, 0.3f);
		Vector3 posOutBoundsOffset = posOut - currentLocalPos;
		if (transform.parent != null)
			posOutBoundsOffset = transform.parent.TransformVector (posOutBoundsOffset);
		Gizmos.DrawWireCube (bounds.center + posOutBoundsOffset, bounds.size);
	}
}
