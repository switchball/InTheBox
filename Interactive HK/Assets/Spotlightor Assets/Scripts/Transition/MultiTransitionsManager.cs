using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiTransitionsManager : TransitionManager
{
	public List<TransitionManager> childTransitions = new List<TransitionManager> ();

	protected override void DoTransitionIn (bool instant, StateTypes prevStateType)
	{
		UnregisterAllTransitionEvents ();

		foreach (TransitionManager child in childTransitions) {
			child.TransitionInCompleted += HandleChildTransitionInCompleted;
			child.TransitionIn (instant);
		}
	}

	void HandleChildTransitionInCompleted (TransitionManager source)
	{
		source.TransitionInCompleted -= HandleChildTransitionInCompleted;

		if (AreChildTransitionsInState (StateTypes.In)) {
			UnregisterAllTransitionEvents ();
			TransitionInComplete ();
		}
	}

	protected override void DoTransitionOut (bool instant, StateTypes prevStateType)
	{
		UnregisterAllTransitionEvents ();

		foreach (TransitionManager child in childTransitions) {
			child.TransitionOutCompleted += HandleChildTransitionOutCompleted;
			child.TransitionOut (instant);
		}
	}

	void HandleChildTransitionOutCompleted (TransitionManager source)
	{
		source.TransitionOutCompleted -= HandleChildTransitionOutCompleted;
		
		if (AreChildTransitionsInState (StateTypes.Out)) {
			UnregisterAllTransitionEvents ();
			TransitionOutComplete ();
		}
	}

	private void UnregisterAllTransitionEvents ()
	{
		foreach (TransitionManager transition in childTransitions) {
			transition.TransitionInCompleted -= HandleChildTransitionInCompleted;
			transition.TransitionOutCompleted -= HandleChildTransitionOutCompleted;
		}
	}

	private bool AreChildTransitionsInState (TransitionManager.StateTypes stateType)
	{
		foreach (TransitionManager child in childTransitions) {
			if (child.State != stateType)
				return false;
		}
		return true;
	}

	void Reset ()
	{
		childTransitions = new List<TransitionManager> ();
		childTransitions.AddRange (GetComponentsInChildren<TransitionManager> (true));
		childTransitions.Remove (this);
	}
}
