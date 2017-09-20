using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Image))]
public class DisplaySpriteByInputMethod : InputMethodResponder
{
	public SpriteByInputMethod spriteByInputMethod;

	protected override void HandleInputMethodChanged (InputMethodTypes newInputMethod, InputMethodTypes oldInputMethod)
	{
		GetComponent<Image> ().sprite = spriteByInputMethod.Current;
	}
}
