using UnityEngine;
using System.Collections;

[RequireComponent (typeof(TransitionManager))]
public class TransitionSoundEffects : MonoBehaviour
{
	public AudioClip transitionInSound;
	public AudioClip transitionInCompletedSound;
	public AudioClip transitionOutSound;
	public AudioClip transitionOutCompletedSound;

	void Awake ()
	{
		TransitionManager transitionManager = GetComponent<TransitionManager> ();
		if (transitionManager != null) {
			transitionManager.TransitionInStarted += OnTransitionInStarted;
			transitionManager.TransitionOutStarted += OnTransitionOutStarted;
			transitionManager.TransitionOutCompleted += HandleTransitionTransitionOutCompleted;
			transitionManager.TransitionInCompleted += HandleTransitionTransitionInCompleted;
		}
	}

	void HandleTransitionTransitionInCompleted (TransitionManager source)
	{
		GlobalSoundPlayer.PlaySound (transitionInCompletedSound);
	}

	void HandleTransitionTransitionOutCompleted (TransitionManager source)
	{
		GlobalSoundPlayer.PlaySound (transitionOutCompletedSound);
	}

	void OnTransitionInStarted (TransitionManager source, bool isInstant, TransitionManager.StateTypes prevStateType)
	{
		if (!isInstant)
			GlobalSoundPlayer.PlaySound (transitionInSound);
	}

	void OnTransitionOutStarted (TransitionManager source, bool isInstant, TransitionManager.StateTypes prevStateType)
	{
		if (!isInstant)
			GlobalSoundPlayer.PlaySound (transitionOutSound);
	}
	
	
}
