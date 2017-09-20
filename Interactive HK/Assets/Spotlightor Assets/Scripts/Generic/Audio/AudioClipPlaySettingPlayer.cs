using UnityEngine;
using System.Collections;

public class AudioClipPlaySettingPlayer : MonoBehaviour
{
	public AudioClipPlaySetting clipSetting;
	public bool playOnAwake = true;

	void OnEnable ()
	{
		if (playOnAwake)
			Play ();
	}

	public void Play ()
	{
		AudioSource audio = GetComponent<AudioSource> ();
		if (audio != null) {
			audio.clip = clipSetting.ClipToPlay;
			audio.volume = clipSetting.volumeScale;
			audio.pitch = clipSetting.pitch;
			audio.Play ();
		} else
			GlobalSoundPlayer.PlaySound (clipSetting);
	}

	void Reset ()
	{
		AudioSource audio = GetComponent<AudioSource> ();
		if (audio != null)
			audio.playOnAwake = false;
	}
}
