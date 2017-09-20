using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class KeyboardEventTrigger : MonoBehaviour
{
	public KeyCode key = KeyCode.Space;
	public UnityEvent pressedEvent;
	public UnityEvent onPressDown;
	public UnityEvent onPressUp;

	void Update ()
	{
		if (Input.GetKeyDown (key))
			pressedEvent.Invoke ();

		if (Input.GetKeyDown (key))
			onPressDown.Invoke ();
		if (Input.GetKeyUp (key))
			onPressUp.Invoke ();
	}
}
