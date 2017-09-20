using UnityEngine;
using System.Collections;

public class MoveToCommand : CoroutineCommandBehavior
{
	public Transform target;
	public Transform destWaypoint;
	public Tweener tweener;
	private Vector3 startPosition;
	private Quaternion startRotation;

	protected override IEnumerator CoroutineCommand ()
	{
		startPosition = target.position;
		startRotation = target.rotation;

		tweener.TimeElapsed = 0;

		while (!tweener.IsCompleted) {
			yield return null;
			tweener.TimeElapsed += Time.deltaTime;

			target.position = Vector3.Lerp (startPosition, destWaypoint.position, tweener.Value);
			target.rotation = Quaternion.Slerp (startRotation, destWaypoint.rotation, tweener.Value);
		}
	}
}
