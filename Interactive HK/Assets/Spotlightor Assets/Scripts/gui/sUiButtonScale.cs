using UnityEngine;
using System.Collections;

[RequireComponent(typeof(sUiButton))]
public class sUiButtonScale : FunctionalMonoBehaviour
{
	public Transform target;
	public Vector3 normalScale = Vector3.one;
	public Vector3 overScale = Vector3.one;
	public Vector3 downScale = Vector3.one;
	public Vector3 disableScale = Vector3.one;
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
		ScaleByButtonState ();
	}
	
	protected override void OnBecameUnFunctional ()
	{
		Button.StateChanged -= HandleStateChanged;
	}
	
	void HandleStateChanged (sUiButton button, sUiButton.StateTypes stateBefore, sUiButton.StateTypes stateNow)
	{
		ScaleByButtonState ();
	}
	
	private void ScaleByButtonState ()
	{
		switch (Button.StateType) {
		case sUiButton.StateTypes.Normal:
			ScaleTo (normalScale);
			break;
		case sUiButton.StateTypes.Over:
			ScaleTo (overScale);
			break;
		case sUiButton.StateTypes.Down:
			ScaleTo (downScale);
			break;
		case sUiButton.StateTypes.Disable:
			ScaleTo (disableScale);
			break;
		}
	}
	
	private void ScaleTo (Vector3 localScale)
	{
		target.localScale = localScale;
	}
	
	void Reset ()
	{
		if (target == null) {
			target = transform;
			normalScale = overScale = downScale = disableScale = target.localScale;
		}
	}
}
