using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenshotsLocalizedCommand : ScreenshotCommand
{
	public List<LocalizationLanguageTypes> languages = new List<LocalizationLanguageTypes> () {
		LocalizationLanguageTypes.Chinese,
		LocalizationLanguageTypes.English
	};

	protected override IEnumerator CoroutineCommand ()
	{
		yield return new WaitForSeconds (delay);

		float timeScaleBefore = Time.timeScale;
		string shotNameBefore = this.screenshotName;

		Time.timeScale = 0;
		foreach (LocalizationLanguageTypes language in languages) {
			Localization.CurrentLanguage = language;
			yield return null;

			this.screenshotName = shotNameBefore + " " + language.ToString ();
			this.Screenshot ();
			yield return null;
		}
		Time.timeScale = timeScaleBefore;
		this.screenshotName = shotNameBefore;
	}
}
