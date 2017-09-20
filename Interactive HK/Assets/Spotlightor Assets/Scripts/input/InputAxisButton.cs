using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class InputAxisButton : MonoBehaviour
{
	public delegate void GenericEventHandler (InputAxisButton button);

	public event GenericEventHandler Clicked;

	public string axisName = "Horizontal";
	public bool invert = false;
	public float repeatDelay = 0.5f;
	public float repeatInterval = 0.2f;
	public bool activeByInputModule = true;
	public UnityEvent onClick;
	private bool isPressed = false;
	private float timeSinceLastClick = 0;
	private int repeatTimes = 0;

	public bool IsPressed {
		get { return isPressed; }
		set {
			if (isPressed != value) {
				isPressed = value;
				if (isPressed)
					OnClicked ();
			}
		}
	}

	public void Update ()
	{
		if (!activeByInputModule || EventSystem.current.currentInputModule.enabled) {
			if (!invert)
				IsPressed = Input.GetAxis (axisName) >= 1;
			else
				IsPressed = Input.GetAxis (axisName) <= -1;
		} else
			IsPressed = false;

		if (IsPressed) {
			timeSinceLastClick += Time.deltaTime;
			float delayTimeForNextClick = repeatTimes == 0 ? repeatDelay : repeatInterval;
			if (timeSinceLastClick >= delayTimeForNextClick) {
				OnClicked ();

				timeSinceLastClick = 0;
				repeatTimes++;
			}
		} else {
			timeSinceLastClick = 0;
			repeatTimes = 0;
		}
	}

	private void OnClicked ()
	{
		if (Clicked != null)
			Clicked (this);
		
		onClick.Invoke ();
	}
}
