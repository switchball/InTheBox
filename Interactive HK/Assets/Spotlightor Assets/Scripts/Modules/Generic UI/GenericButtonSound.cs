using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GenericButton))]
public class GenericButtonSound : MonoBehaviour
{
	public AudioClipPlaySetting press;
	public AudioClipPlaySetting click;

	void Awake ()
	{
		GenericButton button = GetComponent<GenericButton> ();
		button.Pressed += HandlePressed;
		button.Clicked += HandleClicked;
	}

	void HandlePressed (GenericButton button)
	{
		PlaySound (press);
	}

	void HandleClicked (GenericButton button)
	{
		PlaySound (click);
	}

	private void PlaySound (AudioClipPlaySetting sound)
	{
		AudioSource audioSource = GetComponent<AudioSource> ();
		if (audioSource != null)
			audioSource.PlayOneShot (sound);
		else
			GlobalSoundPlayer.PlaySound (sound);
	}
}
