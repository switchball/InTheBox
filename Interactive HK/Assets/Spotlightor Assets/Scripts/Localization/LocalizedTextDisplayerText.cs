using UnityEngine;
using System.Collections;

public class LocalizedTextDisplayerText : LanguageChangeAwareBehavior
{
	public LocalizedText text;
	private TextDisplayer textDisplayer;

	private TextDisplayer TextDisplayer {
		get {
			if (textDisplayer == null) {
				textDisplayer = GetComponent<TextDisplayer> ();
				if (textDisplayer == null)
					textDisplayer = gameObject.AddComponent<GenericTextDisplayer> ();
			}
			return textDisplayer;
		}
	}

	protected override void ResponseToLanguage (LocalizationLanguageTypes language)
	{
		DisplayLocalizedText ();
	}

	public void DisplayLocalizedTextOfKey (string key)
	{
		text.key = key;
		DisplayLocalizedText ();
	}

	public void DisplayLocalizedText ()
	{
		TextDisplayer.Text = text;
	}
	
	public void DisplayLocalizedTextByLanguage (LocalizationLanguageTypes language)
	{
		TextDisplayer.Text = text.ToString (language);
	}
	
}
