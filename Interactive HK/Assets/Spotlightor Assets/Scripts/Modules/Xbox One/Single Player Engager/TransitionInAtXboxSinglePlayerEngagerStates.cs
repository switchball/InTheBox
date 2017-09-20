using UnityEngine;
using System.Collections;

public class TransitionInAtXboxSinglePlayerEngagerStates : TransitionInAtFsmStates<XboxSinglePlayerEngager, XboxSinglePlayerEngager.StateTypes>
{
	protected override Fsm<XboxSinglePlayerEngager, XboxSinglePlayerEngager.StateTypes> FiniteStateMachine {
		get {
			if (Application.platform == RuntimePlatform.XboxOne && XboxSinglePlayerEngager.Instance != null)
				return XboxSinglePlayerEngager.Instance.FiniteStateMachine;
			else
				return null;
		}
	}
}
