using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InputButtonEventTrigger : MonoBehaviour
{
	public string buttonName = "Jump";
	public UnityEvent onButtonDown;
	public UnityEvent onButtonUp;

	void Update ()
	{
		if (Input.GetButtonDown (buttonName))
			onButtonDown.Invoke ();
		if (Input.GetButtonUp (buttonName))
			onButtonUp.Invoke ();
	}
}
