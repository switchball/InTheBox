using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GosActivatorOnGoFsmStates : GosActivatorOnCondition
{
	public List<GoFsmState> activeStates;
	private Fsm<GoFsmController,int> fsm;

	protected override bool HasMetCondition {
		get {
			bool hasMetCondition = false;
			if (fsm != null) {
				foreach (GoFsmState state in activeStates) {
					if (fsm.CurrentState == state) {
						hasMetCondition = true;
					}
				}
			}
			return hasMetCondition;
		}
	}

	void Start ()
	{
		if (activeStates.Count > 0) {
			fsm = activeStates [0].Owner.FiniteStateMachine;

			ActivateObjectsOnCondition ();
			fsm.StateChange += HandleFsmStateChange;
		}
	}

	void OnDisable ()
	{
		if (fsm != null)
			fsm.StateChange -= HandleFsmStateChange;
	}

	void HandleFsmStateChange (int newStateId, int previousStateId)
	{
		ActivateObjectsOnCondition ();
	}
}
