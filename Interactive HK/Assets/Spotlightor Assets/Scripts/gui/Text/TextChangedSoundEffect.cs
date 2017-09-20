using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextDisplayer))]
public class TextChangedSoundEffect : FunctionalMonoBehaviour
{
	public AudioClip textChangedSound;
	public float volume = 1;
	public float minSoundPlayInterval = 0.3f;
	private float lastSoundPlayTime = 0;

	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		GetComponent<TextDisplayer> ().TextChanged += HandleTextChanged;
	}

	protected override void OnBecameUnFunctional ()
	{
		GetComponent<TextDisplayer> ().TextChanged -= HandleTextChanged;
	}
	
	void HandleTextChanged (string text)
	{
		if (textChangedSound != null) {
			if (Time.time - lastSoundPlayTime > minSoundPlayInterval) {
				GlobalSoundPlayer.PlaySound (textChangedSound, volume);
				lastSoundPlayTime = Time.time;
			}
		}
	}
	
}
