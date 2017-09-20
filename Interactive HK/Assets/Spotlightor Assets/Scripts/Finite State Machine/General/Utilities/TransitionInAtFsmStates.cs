using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TransitionInAtFsmStates<T, U> : MonoBehaviour where T:class
{
	public List<U> inStateTypes;
	public List<TransitionManager> transitions;

	protected abstract Fsm<T,U> FiniteStateMachine{ get; }

	void OnEnable ()
	{
		UpdateTransitions ();
		
		if (FiniteStateMachine != null)
			FiniteStateMachine.StateChange += HandleGameSceneStateChange;
	}

	void OnDisable ()
	{
		if (FiniteStateMachine != null)
			FiniteStateMachine.StateChange -= HandleGameSceneStateChange;
	}

	void HandleGameSceneStateChange (U newStateId, U previousStateId)
	{
		UpdateTransitions ();
	}

	private void UpdateTransitions ()
	{
		if (FiniteStateMachine != null && FiniteStateMachine.CurrentState != null) {
			U currentStateType = FiniteStateMachine.CurrentState.StateId;
			if (inStateTypes.IndexOf (currentStateType) >= 0)
				transitions.ForEach (transition => transition.TransitionIn ());
			else
				transitions.ForEach (transition => transition.TransitionOut ());
		}
	}
}