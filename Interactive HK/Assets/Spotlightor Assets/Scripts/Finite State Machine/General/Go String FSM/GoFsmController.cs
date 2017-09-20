using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoFsmController : MonoBehaviour
{
	public GoFsmState initialState;
	private Fsm<GoFsmController, int> finiteStateMachine;

	public Fsm<GoFsmController, int> FiniteStateMachine {
		get {
			if (finiteStateMachine == null) {
				finiteStateMachine = new Fsm<GoFsmController, int> (this);

				List<GoFsmState> states = new List<GoFsmState> (GetComponentsInChildren<GoFsmState> (true));
				if (initialState != null && !states.Contains (initialState))
					states.Add (initialState);

				if (states.Count > 0) {
					states.ForEach (state => state.gameObject.SetActive (false));

					finiteStateMachine.AddStates (states.ToArray ());
					if (initialState == null && states.Count > 0)
						initialState = states [0];

					finiteStateMachine.GotoState (initialState.StateId);
				}
			}
			return finiteStateMachine;
		}
	}

	public void Awake ()
	{
		GotoState (initialState);
	}

	public void GotoState (GoFsmState state)
	{
		FiniteStateMachine.GotoState (state.StateId);
	}

}
