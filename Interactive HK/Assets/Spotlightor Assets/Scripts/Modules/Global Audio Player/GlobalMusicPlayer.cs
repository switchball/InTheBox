using UnityEngine;
using System.Collections;

public class GlobalMusicPlayer : SingletonMonoBehaviour<GlobalMusicPlayer>
{
	private class TransitionData
	{
		public AudioClip audioClip;
		public bool loop = true;
		public float volume = 1;
		public float fadeInTime = 0.5f;
		public float fadeOutTime = 0.5f;
	}

	public bool Mute {
		get { return GetComponent<AudioSource> ().mute; }
		set {
			AudioSource[] audioSources = GetComponents<AudioSource> ();
			foreach (AudioSource audioSource in audioSources)
				audioSource.mute = value;
		}
	}

	protected override void Awake ()
	{
		base.Awake ();

		AudioSource audioSource = gameObject.AddComponent<AudioSource> ();
		audioSource.loop = true;
		audioSource.playOnAwake = false;
		audioSource.volume = 0;
#if UNITY_5
		audioSource.spatialBlend = 0;
		audioSource.bypassReverbZones = true;
		audioSource.reverbZoneMix = 0;
		if (GlobalMusicPlayerSettings.Instance != null)
			audioSource.outputAudioMixerGroup = GlobalMusicPlayerSettings.Instance.output;
#endif

		DontDestroyOnLoad (gameObject);
	}

	public void TransitTo (AudioClip audioClip, bool loop, float volume, float fadeInTime, float fadeOutTime)
	{
		TransitionData transitionData = new TransitionData ();
		transitionData.audioClip = audioClip;
		transitionData.loop = loop;
		transitionData.volume = volume;
		transitionData.fadeInTime = fadeInTime;
		transitionData.fadeOutTime = fadeOutTime;

		StartTransition (transitionData);
	}

	public void Stop (float fadeOutTime)
	{
		TransitionData transitionData = new TransitionData ();
		transitionData.audioClip = null;
		transitionData.loop = true;
		transitionData.volume = 0;
		transitionData.fadeInTime = 0;
		transitionData.fadeOutTime = fadeOutTime;
		
		StartTransition (transitionData);
	}

	private void StartTransition (TransitionData transitionData)
	{
		StopCoroutine ("Transition");
		if (transitionData.fadeInTime == 0 && transitionData.fadeOutTime == 0)
			InstantTransition (transitionData);
		else
			StartCoroutine ("Transition", transitionData);
	}

	private void InstantTransition (TransitionData transitionData)
	{
		AudioClip oldAudioClip = GetComponent<AudioSource> ().clip;
		if (oldAudioClip != transitionData.audioClip) {
			GetComponent<AudioSource> ().Stop ();
			GetComponent<AudioSource> ().clip = transitionData.audioClip;
			GetComponent<AudioSource> ().loop = transitionData.loop;
			if (transitionData.audioClip != null)
				GetComponent<AudioSource> ().Play ();
		}
		GetComponent<AudioSource> ().volume = transitionData.volume;
	}

	private IEnumerator Transition (TransitionData transitionData)
	{
		AudioClip oldAudioClip = GetComponent<AudioSource> ().clip;
		float oldVolume = GetComponent<AudioSource> ().volume;

		if (oldAudioClip != null && oldAudioClip != transitionData.audioClip) {
			Tweener fadeOutVolumeTweener = new Tweener (oldVolume, 0, transitionData.fadeOutTime, iTween.EaseType.linear);
			do {
				yield return null;

				fadeOutVolumeTweener.TimeElapsed += Time.deltaTime;
				GetComponent<AudioSource> ().volume = fadeOutVolumeTweener.Value;
			} while (!fadeOutVolumeTweener.IsCompleted);

			GetComponent<AudioSource> ().Stop ();
			GetComponent<AudioSource> ().clip = null;
		}

		if (transitionData.audioClip != null) {
			if (GetComponent<AudioSource> ().clip != transitionData.audioClip)
				GetComponent<AudioSource> ().clip = transitionData.audioClip;

			Tweener fadeInVolumeTweener = new Tweener (GetComponent<AudioSource> ().volume, transitionData.volume, transitionData.fadeInTime, iTween.EaseType.linear);

			if (!GetComponent<AudioSource> ().isPlaying)
				GetComponent<AudioSource> ().Play ();

			GetComponent<AudioSource> ().loop = transitionData.loop;

			do {
				yield return null;

				fadeInVolumeTweener.TimeElapsed += Time.deltaTime;
				GetComponent<AudioSource> ().volume = fadeInVolumeTweener.Value;
			} while (!fadeInVolumeTweener.IsCompleted);
		}
	}
}
