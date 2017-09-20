using UnityEngine;
using System.Collections;

public class TransitionCommand : CoroutineCommandBehavior
{
	public enum TransitionTypes
	{
		In,
		Out,
		InAndOut,
	}

	public TransitionManager transition;
	public TransitionTypes transitionType;
	public float inAndOutWaitTime = 3;
	[UnityEngine.Serialization.FormerlySerializedAs ("waitForTransitionOut")]
	public bool waitTransitionComplete = true;

	protected override IEnumerator CoroutineCommand ()
	{
		if (transitionType == TransitionTypes.In || transitionType == TransitionTypes.InAndOut) {
			transition.TransitionIn ();
			if (waitTransitionComplete) {
				while (transition.State != TransitionManager.StateTypes.In)
					yield return null;
			}
		}

		if (transitionType == TransitionTypes.InAndOut)
			yield return new WaitForSeconds (inAndOutWaitTime);

		if (transitionType == TransitionTypes.Out || transitionType == TransitionTypes.InAndOut) {
			transition.TransitionOut ();
			if (waitTransitionComplete) {
				while (transition.State != TransitionManager.StateTypes.Out)
					yield return null;
			}
		}
	}
}
