using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalizationSpriteAsset : ScriptableObject
{
	[System.Serializable]
	public class SpriteLanguageDictionary : SerializableDictionary<LocalizationLanguageTypes, Sprite>
	{

	}

	public SpriteLanguageDictionary sprites;

	public Sprite LocalizedSprite { get { return GetSpriteForLanguage (Localization.CurrentLanguage); } }

	public Sprite GetSpriteForLanguage (LocalizationLanguageTypes language)
	{
		Sprite sprite = null;
		sprites.Dictionary.TryGetValue (language, out sprite);
		return sprite;
	}
}
