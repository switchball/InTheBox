using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteByInputMethod : ScriptableObject
{
	[System.Serializable]
	public class SpriteInputMethodDictionary : SerializableDictionary<InputMethodTypes, Sprite>
	{

	}

	public SpriteInputMethodDictionary sprites;

	public Sprite Current { get { return GetSpriteForInputMethod (InputMethodDetector.Instance.CurrentInputMethod); } }

	public Sprite GetSpriteForInputMethod (InputMethodTypes inputMethod)
	{
		Sprite sprite = null;
		sprites.Dictionary.TryGetValue (inputMethod, out sprite);
		return sprite;
	}
}
