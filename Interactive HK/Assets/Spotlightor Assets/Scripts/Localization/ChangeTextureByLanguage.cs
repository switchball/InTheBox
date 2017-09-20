using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class TextureByLangauge:ContentByLanguage
{
	public Texture texture;
}

public class ChangeTextureByLanguage : LanguageSpecifiedContent<TextureByLangauge>
{
	protected override void ActivateContent (TextureByLangauge contentByLanguage)
	{
		GetComponent<Renderer>().material.mainTexture = contentByLanguage.texture;
	}
}
