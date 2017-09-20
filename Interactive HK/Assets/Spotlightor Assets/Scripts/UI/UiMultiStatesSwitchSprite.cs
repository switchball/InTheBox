using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UiMultiStates))]
public class UiMultiStatesSwitchSprite : MonoBehaviour
{
	public Image image;
	public List<Sprite> spriteByStateIds;

	void Start ()
	{
		UpdateObjectActivation (GetComponent<UiMultiStates> ());
		
		GetComponent<UiMultiStates> ().StateChanged += HandleStateChanged;
	}
	
	void HandleStateChanged (UiMultiStates multiStates)
	{
		UpdateObjectActivation (multiStates);
	}
	
	private void UpdateObjectActivation (UiMultiStates multiStates)
	{
		image.sprite = spriteByStateIds [Mathf.Clamp (multiStates.StateId, 0, spriteByStateIds.Count)];
	}
}
