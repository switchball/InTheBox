using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransitionInAtGoFsmStates : MonoBehaviour
{
	public List<GoFsmState> inStates;
	public List<TransitionManager> transitions;

	private Fsm<GoFsmController,int> fsm;

	void Start ()
	{
		if (inStates.Count > 0)
			fsm = inStates [0].Owner.FiniteStateMachine;
		
		UpdateTransitions ();

		fsm.StateChange += HandleFsmStateChange;
	}

	void OnDestroy ()
	{
		if (fsm != null)
			fsm.StateChange -= HandleFsmStateChange;
	}

	void HandleFsmStateChange (int newStateId, int previousStateId)
	{
		UpdateTransitions ();
	}

	private void UpdateTransitions ()
	{
		if (fsm != null && fsm.CurrentState != null) {
			GoFsmState currentState = fsm.CurrentState as GoFsmState;
			if (inStates.IndexOf (currentState) >= 0)
				transitions.ForEach (transition => transition.TransitionIn ());
			else
				transitions.ForEach (transition => transition.TransitionOut ());
		}
	}
}