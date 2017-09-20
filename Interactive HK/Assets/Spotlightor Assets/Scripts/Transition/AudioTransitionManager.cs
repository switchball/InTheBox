using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class AudioTransitionManager : ValueTransitionManager
{
	public enum AutoControlTypes
	{
		None,
		AutoPlayStop,
		AutoPlayPause,
	}

	[System.Serializable]
	public class AudioStateSetting
	{
		[Range (0, 1)]
		public float volume = 1;

		public AudioStateSetting (float volume)
		{
			this.volume = volume;
		}
	}

	public AudioStateSetting inAudioSetting = new AudioStateSetting (1);
	public AudioStateSetting outAudioSetting = new AudioStateSetting (0);
	public AutoControlTypes autoControl = AutoControlTypes.None;

	private AudioSource MyAudio{ get { return GetComponent<AudioSource> (); } }

	protected override void OnProgressValueUpdated (float progress)
	{
		MyAudio.volume = Mathf.Lerp (outAudioSetting.volume, inAudioSetting.volume, progress);
	}

	protected override void Awake ()
	{
		this.TransitionInStarted += HandleTransitionInStarted;
		this.TransitionOutCompleted += HandleTransitionOutCompleted;

		base.Awake ();
	}

	void HandleTransitionInStarted (TransitionManager source, bool isInstant, StateTypes prevStateType)
	{
		if (autoControl != AutoControlTypes.None && !MyAudio.isPlaying)
			MyAudio.Play ();
	}

	void HandleTransitionOutCompleted (TransitionManager source)
	{
		if (autoControl != AutoControlTypes.None && MyAudio.isPlaying) {
			if (autoControl == AutoControlTypes.AutoPlayStop)
				MyAudio.Stop ();
			else if (autoControl == AutoControlTypes.AutoPlayPause)
				MyAudio.Pause ();
		}
	}

	void Reset ()
	{
		this.easeIn = this.easeOut = iTween.EaseType.linear;
	}
}
