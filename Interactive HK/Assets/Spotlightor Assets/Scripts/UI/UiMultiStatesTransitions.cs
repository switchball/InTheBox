using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UiMultiStates))]
public class UiMultiStatesTransitions : MonoBehaviour
{
	public List<int> transitionInStateIds = new List<int>{1};
	public List<TransitionManager> transitions;

	void Start ()
	{
		UpdateTransitions ();
		GetComponent<UiMultiStates> ().StateChanged += HandleStateChanged;
	}

	void HandleStateChanged (UiMultiStates uiMultiStates)
	{
		UpdateTransitions ();
	}

	private void UpdateTransitions ()
	{
		bool transitionIn = transitionInStateIds.Contains (GetComponent<UiMultiStates> ().StateId);
		if (transitionIn)
			transitions.ForEach (t => t.TransitionIn ());
		else
			transitions.ForEach (t => t.TransitionOut ());
	}
}
