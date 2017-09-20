using UnityEngine;
using System.Collections;

public class InputModuleControllerByInputMethod : InputMethodResponder
{
	protected override void HandleInputMethodChanged (InputMethodTypes newInputMethod, InputMethodTypes oldInputMethod)
	{
		PointerToggleStandaloneInputModule module = GetComponent<PointerToggleStandaloneInputModule> ();
		if (module != null)
			module.PointerEnabled = InputMethodDetector.Instance.CurrentInputMethod == InputMethodTypes.KeyboardMouse;
	}
}
