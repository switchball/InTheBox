using UnityEngine;
using System.Collections;

[RequireComponent(typeof(sUiButton))]
public class sUiButtonKeyBinding : MonoBehaviour
{
	public KeyCode keyCode = KeyCode.Alpha1;
	public bool activeWhenTyping = false;

	void Update ()
	{
		if (keyCode != KeyCode.None) {
			if (Input.GetKeyDown (keyCode))
				SimulatePressMessages ();
			else if (Input.GetKeyUp (keyCode))
				SimulateReleaseMessages ();
		}
	}

	void OnGUI ()
	{
		if (activeWhenTyping) {
			if (Event.current.keyCode == keyCode) {
				SimulatePressMessages ();
				SimulateReleaseMessages ();
			}
		}
	}

	private void SimulatePressMessages ()
	{
		gameObject.SendMessage ("OnPressDown", SendMessageOptions.DontRequireReceiver);
		gameObject.SendMessage ("OnHoverIn", SendMessageOptions.DontRequireReceiver);
	}

	private void SimulateReleaseMessages ()
	{
		gameObject.SendMessage ("OnSelect", SendMessageOptions.DontRequireReceiver);
		gameObject.SendMessage ("OnPressUp", SendMessageOptions.DontRequireReceiver);
		gameObject.SendMessage ("OnHoverOut", SendMessageOptions.DontRequireReceiver);
	}
}
