using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MouseEventDispatcher))]
public class sUiButtonObjectActivator : FunctionalMonoBehaviour
{
	public GameObject normalObject;
	public GameObject overObject;
	public GameObject downObject;
	public GameObject disableObject;
	private GameObject activeObject;
	private sUiButton button;
	
	private sUiButton Button {
		get {
			if (button == null)
				button = GetComponent<sUiButton> ();
			return button;
		}
	}

	public GameObject ActiveObject {
		get { return activeObject; }
		set {
			if (normalObject != null)
				normalObject.SetActive (normalObject == value);
			if (overObject != null)
				overObject.SetActive (overObject == value);
			if (downObject != null)
				downObject.SetActive (downObject == value);
			if (disableObject != null)
				disableObject.SetActive (disableObject == value);
			activeObject = value;
		}
	}

	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		Button.StateChanged += HandleButtonStateChanged;
		ActivateGameObjectByButtonState ();
	}

	protected override void OnBecameUnFunctional ()
	{
		Button.StateChanged -= HandleButtonStateChanged;
	}
	
	void HandleButtonStateChanged (sUiButton button, sUiButton.StateTypes stateBefore, sUiButton.StateTypes stateNow)
	{
		ActivateGameObjectByButtonState ();
	}

	private void ActivateGameObjectByButtonState ()
	{
		switch (Button.StateType) {
		case sUiButton.StateTypes.Normal:
			ActiveObject = normalObject;
			break;
		case sUiButton.StateTypes.Over:
			ActiveObject = overObject;
			break;
		case sUiButton.StateTypes.Down:
			ActiveObject = downObject;
			break;
		case sUiButton.StateTypes.Disable:
			ActiveObject = disableObject;
			break;
		}
	}
}
