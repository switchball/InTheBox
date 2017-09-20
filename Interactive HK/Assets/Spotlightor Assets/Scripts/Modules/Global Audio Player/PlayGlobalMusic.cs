using UnityEngine;
using System.Collections;

public class PlayGlobalMusic : MonoBehaviour
{
	public AudioClip audioClip;
	public bool loop = true;
	public float volume = 1;
	public float fadeInTime = 0.5f;
	public float fadeOutTime = 0.5f;

	void OnEnable ()
	{
		GlobalMusicPlayer.Instance.TransitTo (audioClip, loop, volume, fadeInTime, fadeOutTime);
	}

	public void Stop ()
	{
		if (GlobalMusicPlayer.Instance.GetComponent<AudioSource> ().clip == this.audioClip)
			GlobalMusicPlayer.Instance.Stop (fadeOutTime);
	}
}
