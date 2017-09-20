using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[System.Serializable]
public class FontByLanguage : ContentByLanguage
{
	public Font font;
}

public class ChangeFontByLanguage : LanguageSpecifiedContent<FontByLanguage>
{
	private delegate void ChangeFontDelegate (Font font);

	private ChangeFontDelegate changeFont;

	private ChangeFontDelegate ChangeFont {
		get {
			if (GetComponent<Text> () != null)
				changeFont = ChangeUiTextFont;
			else if (GetComponent<TextMesh> () != null)
				changeFont = ChangeTextMeshFont;
			return changeFont;
		}
	}
	
	private void ChangeUiTextFont (Font font)
	{
		GetComponent<Text> ().font = font;
	}

	private void ChangeTextMeshFont (Font font)
	{
		GetComponent<TextMesh> ().font = font;
		GetComponent<Renderer>().material = font.material;
	}

	protected override void ActivateContent (FontByLanguage contentByLanguage)
	{
		ChangeFont (contentByLanguage.font);
	}
}
