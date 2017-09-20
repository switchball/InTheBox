using UnityEngine;
using System.Collections;

[RequireComponent(typeof(sUiButton))]
public class sUiButtonSound : FunctionalMonoBehaviour
{
	[System.Serializable]
	public class SoundSetting
	{
		public AudioClip audioClip;
		public float volume = 1;
		public float pitch = 1;
	}
	public SoundSetting rollOverSound;
	public SoundSetting rollOutSound;
	public SoundSetting mouseDownSound;
	public SoundSetting mouseUpSound;
	public SoundSetting clickSound;
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
		Button.HoverIn += OnRollOverButton;
		Button.HoverOut += OnRollOutButton;
		Button.PressDown += OnMouseDownButton;
		Button.PressUp += OnMouseUpButton;
		Button.Clicked += OnClickedButton;
	}

	protected override void OnBecameUnFunctional ()
	{
		Button.HoverIn -= OnRollOverButton;
		Button.HoverOut -= OnRollOutButton;
		Button.PressDown -= OnMouseDownButton;
		Button.PressUp -= OnMouseUpButton;
		Button.Clicked -= OnClickedButton;
	}

	void OnClickedButton (GenericButton source)
	{
		PlaySound (clickSound);
	}
	
	void OnMouseUpButton (MouseEventDispatcher source)
	{
		PlaySound (mouseUpSound);
	}

	void OnMouseDownButton (MouseEventDispatcher source)
	{
		PlaySound (mouseDownSound);
	}

	void OnRollOutButton (MouseEventDispatcher source)
	{
		PlaySound (rollOutSound);
	}

	void OnRollOverButton (MouseEventDispatcher source)
	{
		PlaySound (rollOverSound);
	}
	
	private void PlaySound (SoundSetting sound)
	{
		if (sound.audioClip != null) 
			GlobalSoundPlayer.PlaySound (sound.audioClip, sound.volume, sound.pitch);
	}
}
