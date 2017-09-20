using UnityEngine;
using System.Collections;

// Customize the supported languages here:
public enum LocalizationLanguageTypes
{
	English = -1,
	Chinese = 0,
	French = 1,
}

public static class Localization
{
	public const string MessageLanguageChanged = "msg_language_changed";
	private const string LanguagePrefsKey = "language_setting";

	public static LocalizationLanguageTypes[] AvailableLanguageTypes {
		get { return LocalizationSettings.Instance.availableLanguageTypes; }
	}

	public static LocalizationLanguageTypes CurrentLanguage {
		get { 
			LocalizationLanguageTypes currentLanguage = LocalizationLanguageTypes.English;
			if (LocalizationSettings.Instance.lockLanguage == false) {
				if (HasLanguagePreference) {
					int languageValue = BasicDataTypeStorage.GetInt (LanguagePrefsKey);
					currentLanguage = (LocalizationLanguageTypes)(languageValue - 1);
				} else if (AvailableLanguageTypes.Length > 0)
					currentLanguage = AvailableLanguageTypes [0];
			} else
				currentLanguage = LocalizationSettings.Instance.lockLanguageType;
			
			return currentLanguage;
		}
		set {
			if (LocalizationSettings.Instance.lockLanguage == false) {
				LocalizationLanguageTypes currentLanguage = CurrentLanguage;
				if (currentLanguage != value) {
					BasicDataTypeStorage.SetInt (LanguagePrefsKey, (int)value + 1);
					Messenger.Broadcast (MessageLanguageChanged, MessengerMode.DONT_REQUIRE_LISTENER);
				}
			}
		}
	}

	public static bool HasLanguagePreference { get { return BasicDataTypeStorage.HasKey (LanguagePrefsKey); } }

	static Localization ()
	{
		
	}

	public static void ClearLanguagePreference ()
	{
		BasicDataTypeStorage.DeleteInt (LanguagePrefsKey);
	}

}