using UnityEngine;
using System.Collections;

public class MoveTransitionWidget : TransitionWidget
{
	public Vector3 posIn = Vector3.one;
	public Vector3 posOut = Vector3.zero;
	public bool isLocal = true;

	public override void UpdateTransitionProgress (float progress)
	{
		Vector3 pos = MathAddons.LerpUncapped (posOut, posIn, progress);
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
}
