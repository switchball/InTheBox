using UnityEngine;
using System.Collections;

public class LocalizationSettings : ScriptableObject
{
	public const string ResourcesPath = "localization_settings";
	public LocalizationLanguageTypes[] availableLanguageTypes;
	public bool lockLanguage = false;
	[HideByBooleanProperty ("lockLanguage", false)]
	public LocalizationLanguageTypes lockLanguageType = LocalizationLanguageTypes.English;
	private static LocalizationSettings instance;

	public static LocalizationSettings Instance {
		get {
			if (instance == null) {
				instance = Resources.Load (ResourcesPath) as LocalizationSettings;
				if (instance == null) {
					instance = new LocalizationSettings ();
					instance.availableLanguageTypes = new LocalizationLanguageTypes[1]{ LocalizationLanguageTypes.English };
					Debug.LogWarning (string.Format ("Cannot find {0} in at {1}", typeof(LocalizationSettings).Name, ResourcesPath));
				}
			}
			return instance;
		}
	}
}
