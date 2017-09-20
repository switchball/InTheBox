using UnityEngine;
using System.Collections;

public abstract class InputMethodResponder : MonoBehaviour
{
	void OnEnable ()
	{
		HandleInputMethodChanged (InputMethodDetector.Instance.CurrentInputMethod, InputMethodTypes.Unkown);
		Messenger.AddListener (InputMethodDetector.MessageInputMethodChanged, OnInputMethodChanged);
	}

	private void OnInputMethodChanged (object data)
	{
		InputMethodTypes oldInputMedhod = (InputMethodTypes)data;
		HandleInputMethodChanged (InputMethodDetector.Instance.CurrentInputMethod, oldInputMedhod);
	}

	protected abstract void HandleInputMethodChanged (InputMethodTypes newInputMethod, InputMethodTypes oldInputMethod);
}
