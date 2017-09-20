using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(LocalizedText), true)]
public class LocalizedTextPropertyDrawer : PropertyDrawer
{
	private float baseHeight;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		float height = base.GetPropertyHeight (property, label);
		baseHeight = height;

		SerializedProperty textsAssetProperty = property.FindPropertyRelative ("textsAsset");
		SerializedProperty keyProperty = property.FindPropertyRelative ("key");
		
		LocalizationTextsAsset textAsset = textsAssetProperty.objectReferenceValue as LocalizationTextsAsset;
		string key = keyProperty.stringValue;

		if (textAsset != null && !string.IsNullOrEmpty (key)) {
			LocalizationText localizationText = textAsset.GetLocalizationTextByKey (key);
			if (localizationText != null)
				height *= 2 + localizationText.textsByLanguages.Length;
			else
				height *= 2;
		} else
			height *= 2;
		return height;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty (position, label, property);

		Rect positionWithLabel = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);

		int intendLevel = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		SerializedProperty textsAssetProperty = property.FindPropertyRelative ("textsAsset");
		LocalizationTextsAsset textAsset = textsAssetProperty.objectReferenceValue as LocalizationTextsAsset;
		SerializedProperty keyProperty = property.FindPropertyRelative ("key");
		string key = keyProperty.stringValue;

		Rect textAssetPosition = positionWithLabel;
		textAssetPosition.height = baseHeight;

		Rect keyPosition = textAssetPosition;
		keyPosition.y += baseHeight;
		keyPosition.width = positionWithLabel.width * 0.5f;

		Rect keyPopupPosition = keyPosition;
		keyPopupPosition.x = keyPosition.xMax;
		keyPopupPosition.width = positionWithLabel.width * 0.2f;

		Rect buttonPosition = keyPopupPosition;
		buttonPosition.x = keyPopupPosition.xMax;
		buttonPosition.width = positionWithLabel.xMax - buttonPosition.x;

		Rect textByLanguagesPosition = position;
		textByLanguagesPosition.yMin = buttonPosition.yMax;

		textsAssetProperty.objectReferenceValue = EditorGUI.ObjectField (textAssetPosition, textAsset, typeof(LocalizationTextsAsset), false);

		keyProperty.stringValue = EditorGUI.TextField (keyPosition, key);

		if (textAsset != null) {
			keyProperty.stringValue = DrawKeyPopup(keyPopupPosition, textAsset, keyProperty.stringValue);

			LocalizationText localizationText = textAsset.GetLocalizationTextByKey (key);
			
			if (localizationText != null) {
				DrawDeleteButton (buttonPosition, textAsset, localizationText);

				DrawTextByLanguages (textByLanguagesPosition, textAsset, localizationText);
			} else {
				if (!string.IsNullOrEmpty (key))
					DrawCreateButton (buttonPosition, textAsset, key);
			}
		}

		EditorGUI.indentLevel = intendLevel;

		EditorGUI.EndProperty ();
	}

	private string DrawKeyPopup (Rect guiPosition, LocalizationTextsAsset textAsset, string currentKey)
	{
		List<string> allKeys = new List<string> ();
		foreach (LocalizationText t in textAsset.texts)
			allKeys.Add (t.key);

		int selectedIndex = allKeys.IndexOf (currentKey);

		selectedIndex = EditorGUI.Popup (guiPosition, selectedIndex, allKeys.ToArray ());
		if (selectedIndex == -1)
			return currentKey;
		else
			return allKeys [selectedIndex];
	}

	private void DrawDeleteButton (Rect guiPosition, LocalizationTextsAsset textAsset, LocalizationText localizationText)
	{
		if (GUI.Button (guiPosition, "Delete")) {
			List<LocalizationText> newTexts = new List<LocalizationText> (textAsset.texts);
			newTexts.Remove (localizationText);
			textAsset.texts = newTexts.ToArray ();
			
			EditorUtility.SetDirty (textAsset);
		}
	}

	private void DrawCreateButton (Rect guiPosition, LocalizationTextsAsset textAsset, string key)
	{
		if (GUI.Button (guiPosition, "Create")) {
			LocalizationText newKeyText = new LocalizationText ();
			newKeyText.key = key;
			
			List<LocalizationText.TextByLanguage> textsByLanguages = new List<LocalizationText.TextByLanguage> ();
			foreach (LocalizationLanguageTypes languageType in Localization.AvailableLanguageTypes) {
				LocalizationText.TextByLanguage languageText = new LocalizationText.TextByLanguage ();
				languageText.language = languageType;
				languageText.text = key;
				textsByLanguages.Add (languageText);
			}
			newKeyText.textsByLanguages = textsByLanguages.ToArray ();
			
			List<LocalizationText> newTexts = new List<LocalizationText> (textAsset.texts);
			newTexts.Add (newKeyText);
			textAsset.texts = newTexts.ToArray ();
			
			EditorUtility.SetDirty (textAsset);
		}
	}

	private void DrawTextByLanguages (Rect position, LocalizationTextsAsset textAsset, LocalizationText localizationText)
	{
		bool dirty = false;

		LocalizationText.TextByLanguage[] textByLanguages = localizationText.textsByLanguages;
		Rect guiPosition = position;
		guiPosition.height /= (float)textByLanguages.Length;
		foreach (LocalizationText.TextByLanguage tbl in textByLanguages) {
			string textFieldText = EditorGUI.TextField (guiPosition, " - " + tbl.language.ToString (), tbl.text);
			if (textFieldText != tbl.text) {
				tbl.text = textFieldText;
				dirty = true;
			}

			guiPosition.y += baseHeight;
		}
		
		if (dirty)
			EditorUtility.SetDirty (textAsset);
	}
}
