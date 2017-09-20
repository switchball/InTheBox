using UnityEngine;
using System.Collections;

public static class GlobalSoundPlayer
{
	private static AudioSource audio;

	private static AudioSource Audio {
		get {
			if (audio == null) {
				GameObject audioGo = new GameObject ("Global Sound Player [Dont Destroy]");
				audio = audioGo.AddComponent<AudioSource> ();
#if UNITY_5
				audio.spatialBlend = 0;
				audio.bypassReverbZones = true;
				audio.reverbZoneMix = 0;
#endif
				GameObject.DontDestroyOnLoad (audioGo);
			}
			return audio;
		}
	}
	
	public static void PlaySound (AudioClipPlaySetting soundSetting)
	{
		PlaySound (soundSetting.ClipToPlay, soundSetting.volumeScale, soundSetting.pitch);
	}

	public static void PlaySound (AudioClip audioClip)
	{
		PlaySound (audioClip, 1, 1);
	}
	
	public static void PlaySound (AudioClip audioClip, float volume)
	{
		PlaySound (audioClip, volume, 1);
	}
	
	public static void PlaySound (AudioClip audioClip, float volume, float pitch)
	{
		if (audioClip != null) 
			Audio.PlayOneShot (audioClip, volume, pitch);
	}
}
