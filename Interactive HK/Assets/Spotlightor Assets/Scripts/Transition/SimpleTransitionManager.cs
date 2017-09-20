using UnityEngine;
using System.Collections;

public class SimpleTransitionManager : TransitionManager
{

	protected override void DoTransitionIn (bool instant, StateTypes prevStateType)
	{
		TransitionInComplete ();
	}

	protected override void DoTransitionOut (bool instant, StateTypes prevStateType)
	{
		TransitionOutComplete ();
	}
	
}
