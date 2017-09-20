using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class sGUI3DButton : sGUISkinableButton
{
	public Renderer buttonRenderer;

	void Awake ()
	{
		if (buttonRenderer == null)
			buttonRenderer = GetComponent<Renderer>();
		if (buttonSkin.normalTexture == null && buttonRenderer.material.mainTexture != null)
			buttonSkin.normalTexture = buttonRenderer.material.mainTexture;
	}

	protected override void ChangeButtonAppearanceUseSkinByState (sGUIButtonSkin skin, sGUIButtonSkin.ButtonState state)
	{
		if (buttonRenderer)
			skin.ChangeButtonAppearanceByState (buttonRenderer.material, state);
	}
	
	private void Reset ()
	{
		if (GetComponent<Collider>() == null)
			gameObject.AddComponent<BoxCollider> ().isTrigger = true;
		if (GetComponent<Renderer>() != null && buttonRenderer.sharedMaterial != null) {
			buttonSkin.normalColor = buttonRenderer.sharedMaterial.color;
			buttonSkin.overColor = buttonRenderer.sharedMaterial.color;
			buttonSkin.downColor = buttonRenderer.sharedMaterial.color;
		} else {
			buttonSkin.normalColor = Color.white;
			buttonSkin.overColor = Color.white;
			buttonSkin.downColor = Color.white;
		}
	}
}

