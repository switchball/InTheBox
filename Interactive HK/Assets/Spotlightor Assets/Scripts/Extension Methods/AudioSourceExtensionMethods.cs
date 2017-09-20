using UnityEngine;
using System.Collections;

public static class AudioSourceExtensionMethods
{
	public static void PlayOneShot (this AudioSource audioSource, AudioClipPlaySetting clipPlaySetting)
	{
		audioSource.PlayOneShot (clipPlaySetting.ClipToPlay, clipPlaySetting.volumeScale, clipPlaySetting.pitch);
	}

	public static void PlayOneShot (this AudioSource audioSource, AudioClip clip, float volumeScale, float pitch)
	{
		if (clip != null) {
			if (pitch != 1) {
				AudioSource tempAudioSource = new GameObject ("temp audio").AddComponent<AudioSource> ();
				tempAudioSource.transform.SetParent (audioSource.transform, false);

				tempAudioSource.pitch = pitch;
			
				tempAudioSource.priority = audioSource.priority;
				tempAudioSource.spread = audioSource.spread;
				tempAudioSource.spatialBlend = audioSource.spatialBlend;
				tempAudioSource.rolloffMode = audioSource.rolloffMode;
				tempAudioSource.spread = audioSource.spread;
				tempAudioSource.maxDistance = audioSource.maxDistance;
				tempAudioSource.minDistance = audioSource.minDistance;
				tempAudioSource.loop = false;
				tempAudioSource.dopplerLevel = audioSource.dopplerLevel;
				tempAudioSource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;

				GameObject.Destroy (tempAudioSource.gameObject, clip.length);
				audioSource = tempAudioSource;
			}
			audioSource.PlayOneShot (clip, volumeScale);
		}
	}
}
