using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MouseEventDispatcher))]
public class sGUISfxByButtonState : MonoBehaviour
{
	[System.Serializable]
	public class SfxSetting
	{
		public AudioClip clip;
		public float pitch = 1;
		public float volumeScale = 1;
	}
	public SfxSetting overSfx;
	public SfxSetting outSfx;
	public SfxSetting downSfx;
	
	// Use this for initialization
	void Start ()
	{
		MouseEventDispatcher mouseEventDispatcher = GetComponent<MouseEventDispatcher> ();
		mouseEventDispatcher.HoverIn += OnRollOverButton;
		mouseEventDispatcher.HoverOut += OnRollOutButton;
		mouseEventDispatcher.PressDown += OnMouseDownOnButton;
	}

	private void OnMouseDownOnButton (MouseEventDispatcher source)
	{
		if (downSfx.clip)
			PlaySfx (downSfx);
	}

	private void OnRollOutButton (MouseEventDispatcher source)
	{
		if (outSfx.clip)
			PlaySfx (outSfx);
	}

	private void OnRollOverButton (MouseEventDispatcher source)
	{
		if (overSfx.clip) 
			PlaySfx (overSfx);
	}
	
	private void PlaySfx (SfxSetting sfx)
	{
		if (GetComponent<AudioSource>()) {
			GetComponent<AudioSource>().pitch = sfx.pitch;
			GetComponent<AudioSource>().PlayOneShot (sfx.clip, sfx.volumeScale);
		} else {
			if (sfx.pitch != 1)
				this.LogWarning ("SFX pitch is not supported without AudioSource component! Attach an AudioSource or set pitch to 1");
			
			AudioSource.PlayClipAtPoint (sfx.clip, transform.position, sfx.volumeScale);
		}
	}
}
