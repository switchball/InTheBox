using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(GenericButton))]
public class UiMultiStatesLoopByClick : MonoBehaviour
{
	void Start ()
	{
		GetComponent<GenericButton> ().Clicked += HandleClicked;
	}

	void HandleClicked (GenericButton button)
	{
		GetComponent<UiMultiStates> ().StateId++;
	}
}
