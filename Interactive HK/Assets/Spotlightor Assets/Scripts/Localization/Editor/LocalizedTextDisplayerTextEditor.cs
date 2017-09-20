using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CustomEditor (typeof(LocalizedTextDisplayerText))]
public class LocalizedTextDisplayerTextEditor : Editor
{
	private LocalizedTextDisplayerText Target {
		get { return target as LocalizedTextDisplayerText; }
	}

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		if (Target != null && Target.text.textsAsset != null && Target.text.textsAsset.GetLocalizationTextByKey (Target.text.key) != null) {
			foreach (LocalizationLanguageTypes language in System.Enum.GetValues(typeof(LocalizationLanguageTypes))) {
				LocalizationText localizationText = Target.text.textsAsset.GetLocalizationTextByKey (Target.text.key);
				bool hasTextOfLanguage = Array.Find<LocalizationText.TextByLanguage> (localizationText.textsByLanguages, element => element.language == language) != null;
				if (hasTextOfLanguage) {
					if (GUILayout.Button (string.Format ("Update Text - {0}", language.ToString ()))) {
						Component textDisplayComponent = null;
						textDisplayComponent = Target.GetComponent<Text> ();
						if (textDisplayComponent == null)
							textDisplayComponent = Target.GetComponent<TextMesh> ();
						if (textDisplayComponent == null)
							textDisplayComponent = Target.GetComponent<GUIText> ();

						if (textDisplayComponent != null)
							Undo.RecordObject (textDisplayComponent, "Update Localized Text");
						
						Target.DisplayLocalizedTextByLanguage (language);
					}
				}
			}
		}
	}
}
