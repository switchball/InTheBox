using UnityEngine;
using System.Collections;

[RequireComponent(typeof(sUiButton))]
public class sUiButtonColor : FunctionalMonoBehaviour
{
	public GameObject target;
	public Color normalColor = Color.white;
	public Color overColor = Color.white;
	public Color downColor = Color.white;
	public Color disableColor = Color.white;
	public bool tintVertexColor = true;
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

		ChangeColorByButtonState ();
	}
	
	protected override void OnBecameUnFunctional ()
	{
		Button.StateChanged -= HandleStateChanged;
	}

	void HandleStateChanged (sUiButton button, sUiButton.StateTypes stateBefore, sUiButton.StateTypes stateNow)
	{
		ChangeColorByButtonState ();
	}

	public void ChangeColorByButtonState ()
	{
		switch (Button.StateType) {
		case sUiButton.StateTypes.Normal:
			{
				ChangeColor (normalColor);
				break;
			}
		case sUiButton.StateTypes.Over:
			{
				ChangeColor (overColor);
				break;
			}
		case sUiButton.StateTypes.Down:
			{
				ChangeColor (downColor);
				break;
			}
		case sUiButton.StateTypes.Disable:
			{
				ChangeColor (disableColor);
				break;
			}
		}
	}
	
	private void ChangeColor (Color targetColor)
	{
		if (target != null) {
			if (tintVertexColor) {
				MeshFilter meshFilter = target.GetComponent<MeshFilter> ();
				if (meshFilter != null && meshFilter.mesh != null) 
					MeshUtility.Tint (meshFilter.mesh, targetColor);
			} else {
				TextMesh textMesh = target.GetComponent<TextMesh> ();
				if (textMesh != null)
					textMesh.color = targetColor;
				else if (target.GetComponent<Renderer>() != null && target.GetComponent<Renderer>().material != null)
					target.GetComponent<Renderer>().material.color = targetColor;
			}
		}
	}
}
