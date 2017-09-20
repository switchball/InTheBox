using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalizedAudioClip : ScriptableObject
{
	[System.Serializable]
	public class AudioClipByLanguage : SerializableEnumKeyDictionary<LocalizationLanguageTypes, AudioClip>
	{
		
	}

	[SerializeField]
	private AudioClipByLanguage clipByLanguages;

	public AudioClip CurrentLanguageClip {
		get {
			AudioClip clip = null;
			clipByLanguages.Dictionary.TryGetValue (Localization.CurrentLanguage, out clip);
			if (clip == null && clipByLanguages.values.Count > 0)
				clip = clipByLanguages.values [0];
			return clip;
		}
	}

	public static implicit operator AudioClip (LocalizedAudioClip localizedAudioClip)
	{
		return localizedAudioClip.CurrentLanguageClip;
	}

	void Reset ()
	{
		if (clipByLanguages.keys == null || clipByLanguages.keys.Count == 0) {
			clipByLanguages.keys = new List<LocalizationLanguageTypes> (Localization.AvailableLanguageTypes);
			clipByLanguages.values = new List<AudioClip> (Localization.AvailableLanguageTypes.Length);
		}
	}
}
