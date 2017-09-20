using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class CharacterSizeByLanguage : ContentByLanguage
{
	public float characterSize = 0.01f;
	public int fontSize = 0;
}

[RequireComponent(typeof(TextMesh))]
public class ChangeCharacterSizeByLanguage : LanguageSpecifiedContent<CharacterSizeByLanguage>
{
	protected override void ActivateContent (CharacterSizeByLanguage contentByLanguage)
	{
		GetComponent<TextMesh> ().characterSize = contentByLanguage.characterSize;
		GetComponent<TextMesh> ().fontSize = contentByLanguage.fontSize;
	}

}
