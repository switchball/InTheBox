using UnityEngine;
using System.Collections;

public class ChangeInputMethod : MonoBehaviour
{
	public InputMethodTypes inputMethod = InputMethodTypes.JoystickXbox;

	void OnEnable ()
	{
		InputMethodDetector.Instance.ForceChangeInputMethod (inputMethod);
	}
}
