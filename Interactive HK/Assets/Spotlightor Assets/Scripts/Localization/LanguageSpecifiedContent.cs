using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class ContentByLanguage
{
	public LocalizationLanguageTypes language;
}

public abstract class LanguageSpecifiedContent<T> : LanguageChangeAwareBehavior where T : ContentByLanguage
{
	public T[] contentByLanguages;

	protected override void ResponseToLanguage (LocalizationLanguageTypes language)
	{
		T contentByLanguage = Array.Find<T> (contentByLanguages, element => element.language == language);
		if (contentByLanguage != null)
			ActivateContent (contentByLanguage);
	}

	protected abstract void ActivateContent (T contentByLanguage);
}
