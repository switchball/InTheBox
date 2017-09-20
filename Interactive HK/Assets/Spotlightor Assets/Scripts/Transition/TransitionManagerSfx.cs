using UnityEngine;
using System.Collections;

public class TransitionManagerSfx : MonoBehaviour
{
	public TransitionManager transition;
	public AudioClipPlaySetting inSound;
	public AudioClipPlaySetting inCompletedSound;
	public AudioClipPlaySetting outSound;
	public AudioClipPlaySetting outCompletedSound;
	private AudioSource myAudio;

	void Awake ()
	{
		if (transition == null)
			transition = GetComponent<TransitionManager> ();
		
		transition.TransitionInStarted += OnTransitionInStarted;
		transition.TransitionOutStarted += OnTransitionOutStarted;
		transition.TransitionOutCompleted += HandleTransitionTransitionOutCompleted;
		transition.TransitionInCompleted += HandleTransitionTransitionInCompleted;

		myAudio = GetComponent<AudioSource> ();
	}

	void OnTransitionInStarted (TransitionManager source, bool isInstant, TransitionManager.StateTypes prevStateType)
	{
		if (!isInstant)
			PlaySound (inSound);
	}

	void HandleTransitionTransitionInCompleted (TransitionManager source)
	{
		PlaySound (inCompletedSound);
	}

	void OnTransitionOutStarted (TransitionManager source, bool isInstant, TransitionManager.StateTypes prevStateType)
	{
		if (!isInstant)
			PlaySound (outSound);
	}

	void HandleTransitionTransitionOutCompleted (TransitionManager source)
	{
		PlaySound (outCompletedSound);
	}

	private void PlaySound (AudioClipPlaySetting clipPlaySetting)
	{
		if (myAudio != null)
			myAudio.PlayOneShot (clipPlaySetting);
		else
			GlobalSoundPlayer.PlaySound (clipPlaySetting);
	}

	void Reset ()
	{
		if (transition == null)
			transition = GetComponent<TransitionManager> ();
	}
}
