using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TransitionManager))]
public class EnableSUiButtonsByTransition : MonoBehaviour
{
	private TransitionManager transitionManager;
	private sUiButton[] buttons;
	
	void Awake ()
	{
		buttons = GetComponentsInChildren<sUiButton> (true);
		transitionManager = GetComponent<TransitionManager> ();
		
		SetAllButtonsEnabled (transitionManager.State == TransitionManager.StateTypes.In || transitionManager.State == TransitionManager.StateTypes.Unkown);
		
		transitionManager.TransitionInCompleted += HandleTransitionInCompleted;
		transitionManager.TransitionOutTriggered += HandleTransitionOutTriggered;
	}
	
	void HandleTransitionInCompleted (TransitionManager source)
	{
		SetAllButtonsEnabled (true);
	}

	void HandleTransitionOutTriggered (TransitionManager source, bool isInstant, TransitionManager.StateTypes prevStateType)
	{
		SetAllButtonsEnabled (false);
	}

	private void SetAllButtonsEnabled (bool buttonEnabled)
	{
		foreach (sUiButton button in buttons)
			button.ButtonEnabled = buttonEnabled;
	}
}
