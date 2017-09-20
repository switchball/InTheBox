using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent (typeof(GenericButton))]
public class GenericButtonClickByInput : MonoBehaviour
{
	public bool triggerOnce = true;
	public string inputName = "Submit";
	public bool activeByUiInput = true;
	private int clickTimes = 0;

	void OnEnable ()
	{
		clickTimes = 0;
	}

	void Update ()
	{
		if (!triggerOnce || clickTimes < 1) {
			bool inputActive = true;
			if (activeByUiInput)
				inputActive = EventSystem.current != null && EventSystem.current.currentInputModule != null && EventSystem.current.currentInputModule.isActiveAndEnabled;
			
			if (inputActive) {
				if (Input.GetButtonDown (inputName)) {
					SendMessage ("OnClicked");
					clickTimes++;
				}
			}
		}
	}
}
