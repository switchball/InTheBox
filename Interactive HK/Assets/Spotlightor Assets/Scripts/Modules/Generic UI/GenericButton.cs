using UnityEngine;
using System.Collections;

public abstract class GenericButton : MonoBehaviour
{
	public delegate void BasicEventHandler (GenericButton button);

	public event BasicEventHandler Pressed;
	public event BasicEventHandler Clicked;

	public abstract bool Interactable{ get; set; }

	public bool IsClicked{ get; private set; }

	protected void OnClicked ()
	{
		if (Interactable) {
			IsClicked = true;
			if (this.isActiveAndEnabled)
				StartCoroutine ("ClearIsClicked");

			if (Clicked != null)
				Clicked (this);
		}
	}

	protected void OnPressed ()
	{
		if (Interactable) {
			if (Pressed != null)
				Pressed (this);
		}
	}

	void OnDisable ()
	{
		StopCoroutine ("ClearIsClicked");
	}

	private IEnumerator ClearIsClicked ()
	{
		yield return null;
		IsClicked = false;
	}
}
