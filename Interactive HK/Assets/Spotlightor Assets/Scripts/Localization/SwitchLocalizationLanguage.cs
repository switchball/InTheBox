using UnityEngine;
using System.Collections;

public class SwitchLocalizationLanguage : MonoBehaviour
{
	public bool switchOnAwake = true;
	public LocalizationLanguageTypes targetLanguage = LocalizationLanguageTypes.Chinese;

	void Awake ()
	{
		if (switchOnAwake)
			SwitchLanguage ();
	}

	public void SwitchLanguage ()
	{
		Localization.CurrentLanguage = targetLanguage;
	}
}
