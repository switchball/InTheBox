using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class LocalizationText
{
	[System.Serializable]
	public class TextByLanguage
	{
		public LocalizationLanguageTypes language;
		public string text;
	}

	public string key;
	public TextByLanguage[] textsByLanguages;

	public string GetLocalizedTextByLanguage (LocalizationLanguageTypes language)
	{
		TextByLanguage textByLanguage = Array.Find (textsByLanguages, element => element.language == language);
		if (textByLanguage != null)
			return textByLanguage.text;
		else
			return "";
	}

	public void SetLocalizedTextByLanguage (LocalizationLanguageTypes language, string text)
	{
		TextByLanguage textByLanguage = Array.Find (textsByLanguages, element => element.language == language);
		if (textByLanguage != null)
			textByLanguage.text = text;
		else
			Debug.LogWarning ("Cannot find localized text for language: " + language.ToString ());
	}

	public string GetAllTexts ()
	{
		string allTexts = "";
		foreach (TextByLanguage textByLanguage in textsByLanguages)
			allTexts += textByLanguage.text;
		return allTexts;
	}

	public override string ToString ()
	{
		return GetLocalizedTextByLanguage (Localization.CurrentLanguage);
	}

	public static implicit operator string (LocalizationText t)
	{
		return t.ToString ();
	}
}

public class LocalizationTextsAsset : ScriptableObject
{
	[System.Serializable]
	public class FontOfLanguage
	{
		public LocalizationLanguageTypes language;
		public Font[] fonts;
	}

	public Font[] fonts;
	public FontOfLanguage[] fontOfLanguages;
	public string additionalChars;
	public TextAsset textAsset;
	public LocalizationText[] texts;

	public int TextsCount {
		get { return texts != null ? texts.Length : 0; }
	}

	public string GetLocalizedTextStringByIndex (int index)
	{
		LocalizationText text = GetLocalizationTextByIndex (index);
		return text == null ? "NULL" : text;
	}

	public LocalizationText GetLocalizationTextByIndex (int index)
	{
		if (texts != null && texts.Length > 0 && index >= 0 && index < texts.Length) {
			return texts [index];
		}
		return null;
	}

	public string GetLocalizedTextStringByKey (string key)
	{
		LocalizationText text = GetLocalizationTextByKey (key);
		return text == null ? "NULL" : text;
	}

	public LocalizationText GetLocalizationTextByKey (string key)
	{
		if (string.IsNullOrEmpty (key))
			return null;
		else
			return System.Array.Find<LocalizationText> (texts, element => element.key == key);
	}

	public string GetAllTextsByLanguage (LocalizationLanguageTypes language)
	{
		string sum = additionalChars;
		foreach (LocalizationText lt in texts) {
			string text = lt.GetLocalizedTextByLanguage (language);
			for (int i = 0; i < text.Length; i++) {
				if (sum.IndexOf (text [i]) == -1)
					sum += text [i];
			}
		}
		return sum;
	}

	public string GetAllTexts ()
	{
		string sum = additionalChars;
		foreach (LocalizationText lt in texts) {
			string text = lt.GetAllTexts ();
			for (int i = 0; i < text.Length; i++) {
				if (sum.IndexOf (text [i]) == -1)
					sum += text [i];
			}
		}
		return sum;
	}
}
