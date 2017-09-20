using UnityEngine;
using System.Collections;

public class WaitTransitionCommand : CoroutineCommandBehavior
{
	public TransitionManager transition;
	public TransitionManager.StateTypes targetState;
	
	protected override IEnumerator CoroutineCommand ()
	{
		while (transition.State != targetState)
			yield return null;
	}
}
