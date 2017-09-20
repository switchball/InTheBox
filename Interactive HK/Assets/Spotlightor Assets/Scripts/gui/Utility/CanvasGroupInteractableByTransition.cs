using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TransitionManager))]
public class CanvasGroupInteractableByTransition : MonoBehaviour
{
	private CanvasGroup canvasGroup;

	void Start ()
	{
		canvasGroup = GetComponent<CanvasGroup> ();

		canvasGroup.interactable = GetComponent<TransitionManager> ().State == TransitionManager.StateTypes.In;

		GetComponent<TransitionManager> ().TransitionInCompleted += HandleTransitionInCompleted;
		GetComponent<TransitionManager> ().TransitionOutTriggered += HandleTransitionOutTriggered;
	}

	void HandleTransitionOutTriggered (TransitionManager source, bool isInstant, TransitionManager.StateTypes prevStateType)
	{
		canvasGroup.interactable = false;
	}

	void HandleTransitionInCompleted (TransitionManager source)
	{
		canvasGroup.interactable = true;
	}
}
