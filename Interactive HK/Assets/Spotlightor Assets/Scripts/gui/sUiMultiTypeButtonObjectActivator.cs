using UnityEngine;
using System.Collections;

public class sUiMultiTypeButtonObjectActivator : MonoBehaviour {

	public GameObject[] objectOfButtonTypes;

	// Use this for initialization
	void Start ()
	{
		ActivateObjectByButtonType (GetComponent<sUiMultiTypeButton> ());
		GetComponent<sUiMultiTypeButton> ().ButtonTypeChanged += HandleButtonTypeChanged;
	}
	
	void HandleButtonTypeChanged (sUiMultiTypeButton button)
	{
		ActivateObjectByButtonType (button);
	}
	
	private void ActivateObjectByButtonType (sUiMultiTypeButton button)
	{
		for(int i = 0; i < objectOfButtonTypes.Length; i++)
			objectOfButtonTypes[i].SetActive(i == button.ButtonType);
	}
}
