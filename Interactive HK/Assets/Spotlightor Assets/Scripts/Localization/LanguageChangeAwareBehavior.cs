using UnityEngine;
using System.Collections;

public abstract class LanguageChangeAwareBehavior : FunctionalMonoBehaviour
{
	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		ResponseToLanguage (Localization.CurrentLanguage);
		Messenger.AddListener (Localization.MessageLanguageChanged, OnLanguageChanged);
	}

	protected override void OnBecameUnFunctional ()
	{
		Messenger.RemoveListener (Localization.MessageLanguageChanged, OnLanguageChanged);
	}

	private void OnLanguageChanged (object data)
	{
		ResponseToLanguage (Localization.CurrentLanguage);
	}

	[ContextMenu("Response English")]
	public void ResponseToEnglish ()
	{
		ResponseToLanguage (LocalizationLanguageTypes.English);
	}

	[ContextMenu("Response Chinese")]
	public void ResponseToChinese ()
	{
		ResponseToLanguage (LocalizationLanguageTypes.Chinese);
	}

	protected abstract void ResponseToLanguage (LocalizationLanguageTypes language);
}
