using UnityEngine;
using System.Collections;

public class ChildrenTransitionsManager : MultiTransitionsManager
{
	public float childDelayInStep = 0;
	public float childDelayOutStep = 0;

	protected override void DoTransitionIn (bool instant, StateTypes prevStateType)
	{
		SetupChildTransitions ();
		base.DoTransitionIn (instant, prevStateType);
	}

	protected override void DoTransitionOut (bool instant, StateTypes prevStateType)
	{
		SetupChildTransitions ();
		base.DoTransitionOut (instant, prevStateType);
	}

	private void SetupChildTransitions ()
	{
		childTransitions.Clear ();
		for (int i = 0; i < transform.childCount; i++) {
			TransitionManager childTransition = transform.GetChild (i).GetComponent<TransitionManager> ();
			if (childTransition != null) {
				childTransitions.Add (childTransition);
				childTransition.delayIn = childDelayInStep * (float)i;
				childTransition.delayOut = childDelayOutStep * (float)i;
			}
		}
	}
}
