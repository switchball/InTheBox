using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MouseEventDispatcher))]
public class sUiButtonMesh : FunctionalMonoBehaviour
{
	public MeshFilter target;
	public Mesh overMesh;
	public Mesh downMesh;
	public Mesh disableMesh;
	private Mesh normalMesh;
	private sUiButton button;
	
	public Mesh NormalMesh {
		get {
			if (normalMesh == null)
				normalMesh = target.sharedMesh;
			return normalMesh;
		}
	}
	
	private sUiButton Button {
		get {
			if (button == null)
				button = GetComponent<sUiButton> ();
			return button;
		}
	}
	
	void Awake ()
	{
		normalMesh = target.sharedMesh;
	}

	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		Button.StateChanged += HandleButtonStateChanged;
		ChangeMeshByButtonState ();
	}

	protected override void OnBecameUnFunctional ()
	{
		Button.StateChanged -= HandleButtonStateChanged;
	}
	
	void HandleButtonStateChanged (sUiButton button, sUiButton.StateTypes stateBefore, sUiButton.StateTypes stateNow)
	{
		ChangeMeshByButtonState ();
	}

	private void ChangeMeshByButtonState ()
	{
		switch (Button.StateType) {
		case sUiButton.StateTypes.Normal:
			ChangeMesh (normalMesh);
			break;
		case sUiButton.StateTypes.Over:
			ChangeMesh (overMesh);
			break;
		case sUiButton.StateTypes.Down:
			ChangeMesh (downMesh);
			break;
		case sUiButton.StateTypes.Disable:
			ChangeMesh (disableMesh);
			break;
		}
	}

	private void ChangeMesh (Mesh newMesh)
	{
		target.mesh = newMesh != null ? newMesh : NormalMesh;
	}
}
