using UnityEngine;
using System.Collections;

[RequireComponent(typeof(sUiButton))]
public class sUiButtonOffset : FunctionalMonoBehaviour
{
	public Transform target;
	public Vector3 normalLocalPos = Vector3.zero;
	public Vector3 overLocalPos = Vector3.zero;
	public Vector3 downLocalPos = Vector3.zero;
	public Vector3 disableLocalPos = Vector3.zero;
	private sUiButton button;

	private sUiButton Button {
		get {
			if (button == null)
				button = GetComponent<sUiButton> ();
			return button;
		}
	}

	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		Button.StateChanged += HandleStateChanged;
		SetLocalPositionByButtonState ();
	}

	protected override void OnBecameUnFunctional ()
	{
		Button.StateChanged -= HandleStateChanged;
	}
	
	void HandleStateChanged (sUiButton button, sUiButton.StateTypes stateBefore, sUiButton.StateTypes stateNow)
	{
		SetLocalPositionByButtonState ();
	}

	private void SetLocalPositionByButtonState ()
	{
		switch (Button.StateType) {
		case sUiButton.StateTypes.Normal:
			target.localPosition = normalLocalPos;
			break;
		case sUiButton.StateTypes.Over:
			target.localPosition = overLocalPos;
			break;
		case sUiButton.StateTypes.Down:
			target.localPosition = downLocalPos;
			break;
		case sUiButton.StateTypes.Disable:
			target.localPosition = disableLocalPos;
			break;
		}
	}
}
