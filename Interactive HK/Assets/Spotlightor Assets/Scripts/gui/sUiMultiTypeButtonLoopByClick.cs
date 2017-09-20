using UnityEngine;
using System.Collections;

[RequireComponent(typeof(sUiMultiTypeButton))]
public class sUiMultiTypeButtonLoopByClick : MonoBehaviour
{
	void Start ()
	{
		GetComponent<sUiMultiTypeButton> ().Clicked += OnClickMultiTypeButton;
	}

	void OnClickMultiTypeButton (GenericButton source)
	{
		sUiMultiTypeButton button = source as sUiMultiTypeButton;
		button.ButtonType++;
	}
}
