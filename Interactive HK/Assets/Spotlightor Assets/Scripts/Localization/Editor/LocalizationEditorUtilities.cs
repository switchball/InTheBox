using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LocalizationEditorUtilities : ScriptableObject
{
	private const string LocalizationAssetsDefaultPath = "Assets";

	[MenuItem ("Spotlightor/Localization/Switch Language/Chinese")]
	public static void SwitchLanguageChinese ()
	{
		SwitchLanguage (LocalizationLanguageTypes.Chinese);
	}

	[MenuItem ("Spotlightor/Localization/Switch Language/English")]
	public static void SwitchLanguageEnglish ()
	{
		SwitchLanguage (LocalizationLanguageTypes.English);
	}

	[MenuItem ("Spotlightor/Localization/Switch Language/French")]
	public static void SwitchLanguageFrench ()
	{
		SwitchLanguage (LocalizationLanguageTypes.French);
	}

	public static void SwitchLanguage (LocalizationLanguageTypes language)
	{
		Localization.CurrentLanguage = language;
		if (SaveSlotsStorage.ActiveSaveSlot != null)
			SaveSlotsStorage.ActiveSaveSlot.Save ();
	}

	[MenuItem ("Spotlightor/Localization/Create Localization Asset")]
	public static void CreateLocalizationAsset ()
	{
		Object data = ScriptableObject.CreateInstance<LocalizationTextsAsset> ();
		string path = LocalizationAssetsDefaultPath + "/" + "localization.asset";
		AssetDatabase.CreateAsset (data, path);
		Selection.activeObject = AssetDatabase.LoadAssetAtPath (path, typeof(LocalizationTextsAsset));
	}

	[MenuItem ("Spotlightor/Localization/Create Localization Setting Asset")]
	public static void CreateLocalizationSettingsAsset ()
	{
		Object data = ScriptableObject.CreateInstance<LocalizationSettings> ();
		string path = LocalizationAssetsDefaultPath + "/" + "localization_settings.asset";
		AssetDatabase.CreateAsset (data, path);
		Selection.activeObject = AssetDatabase.LoadAssetAtPath (path, typeof(LocalizationSettings));
	}

	[MenuItem ("Spotlightor/Localization/Update fonts for ALL")]
	public static void UpdateFontCharactersForAll ()
	{
		List<TrueTypeFontImporter> modifiedFontImporters = new List<TrueTypeFontImporter> ();
		List<LocalizationTextsAsset> assets = LoadAllLocalizationTextsAssets ();
		foreach (LocalizationTextsAsset asset in assets) {
			if (asset.fonts != null && asset.fonts.Length > 0) {
				string assetAllText = asset.GetAllTexts ();
				foreach (Font assetFont in asset.fonts) {
					TrueTypeFontImporter fontImporter = TrueTypeFontImporter.GetAtPath (AssetDatabase.GetAssetPath (assetFont)) as TrueTypeFontImporter;
					if (modifiedFontImporters.Contains (fontImporter) == false) {
						fontImporter.customCharacters = "";
						modifiedFontImporters.Add (fontImporter);
					}

					for (int i = 0; i < assetAllText.Length; i++) {
						if (fontImporter.customCharacters.IndexOf (assetAllText [i]) == -1)
							fontImporter.customCharacters += assetAllText [i];
					}
				}
			}
			if (asset.fontOfLanguages != null && asset.fontOfLanguages.Length > 0) {
				foreach (LocalizationTextsAsset.FontOfLanguage fontOfLanguage in asset.fontOfLanguages) {
					string textsOfLanguage = asset.GetAllTextsByLanguage (fontOfLanguage.language);
					foreach (Font assetFont in fontOfLanguage.fonts) {
						TrueTypeFontImporter fontImporter = TrueTypeFontImporter.GetAtPath (AssetDatabase.GetAssetPath (assetFont)) as TrueTypeFontImporter;
						if (modifiedFontImporters.Contains (fontImporter) == false) {
							fontImporter.customCharacters = "";
							modifiedFontImporters.Add (fontImporter);
						}
						
						for (int i = 0; i < textsOfLanguage.Length; i++) {
							if (fontImporter.customCharacters.IndexOf (textsOfLanguage [i]) == -1)
								fontImporter.customCharacters += textsOfLanguage [i];
						}
					}
				}
			}
		}
		foreach (TrueTypeFontImporter fontImporter in modifiedFontImporters) {
			Debug.Log ("Reimport modified Font " + fontImporter.assetPath + " with characters count: " + fontImporter.customCharacters.Length.ToString ());
			AssetDatabase.ImportAsset (fontImporter.assetPath);
		}
		modifiedFontImporters = null;
	}

	[MenuItem ("Spotlightor/Localization/Log for excel")]
	public static void LogLocalizationTextsForExcel ()
	{
		List<LocalizationTextsAsset> assets = LoadAllLocalizationTextsAssets ();
		string logMsg = "";
		foreach (LocalizationTextsAsset asset in assets) {
			logMsg += asset.name + "\n";
			for (int i = 0; i < asset.TextsCount; i++) {
				LocalizationText localizationText = asset.GetLocalizationTextByIndex (i);
				List<LocalizationText.TextByLanguage> languageTexts = new List<LocalizationText.TextByLanguage> (localizationText.textsByLanguages);
				languageTexts.Sort ((x, y) => ((int)x.language).CompareTo ((int)y.language));
				for (int j = 0; j < languageTexts.Count; j++)
					logMsg += string.Format ("\t\"{0}\"", languageTexts [j].text);
				logMsg += "\n";
			}
		}
		Debug.Log (logMsg);
	}

	private static List<LocalizationTextsAsset> LoadAllLocalizationTextsAssets ()
	{
		List<LocalizationTextsAsset> result = new List<LocalizationTextsAsset> ();
		string[] allAssetPaths = AssetDatabase.GetAllAssetPaths ();
		foreach (string assetPath in allAssetPaths) {
			if (assetPath.Length > 5 && assetPath.Substring (assetPath.Length - 5) == "asset") {
				LocalizationTextsAsset asset = AssetDatabase.LoadAssetAtPath (assetPath, typeof(LocalizationTextsAsset)) as LocalizationTextsAsset;
				if (asset != null)
					result.Add (asset);
			}
		}
		Debug.Log (string.Format ("Load {0} LocalizationTextsAsset.", result.Count));
		return result;
	}
}
