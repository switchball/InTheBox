using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GenericButtonEventTrigger : MonoBehaviour
{
	public GenericButton button;
	public UnityEvent clicked;

	void Start ()
	{
		button.Clicked += HandleButtonClicked;
	}

	void HandleButtonClicked (GenericButton button)
	{
		clicked.Invoke ();
	}

	void Reset ()
	{
		if (button == null)
			button = GetComponentInChildren<GenericButton> ();
	}
}
