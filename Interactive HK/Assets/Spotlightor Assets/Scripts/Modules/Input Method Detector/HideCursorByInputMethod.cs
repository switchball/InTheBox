using UnityEngine;
using System.Collections;

public class HideCursorByInputMethod : InputMethodResponder
{
	protected override void HandleInputMethodChanged (InputMethodTypes newInputMethod, InputMethodTypes oldInputMethod)
	{
		Cursor.visible = newInputMethod == InputMethodTypes.KeyboardMouse;
	}
}
