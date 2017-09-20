using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HangOnGame : MonoBehaviour
{
	public bool zeroTimeScale = true;
	public bool disableUiInput = true;
	private float timeScaleBefore = 1;
	private GameObject selectedGoBefore = null;

	void OnEnable ()
	{
		if (zeroTimeScale) {
			timeScaleBefore = Time.timeScale;
			Time.timeScale = 0;
		}
		
		if (disableUiInput) {
			if (EventSystem.current != null && EventSystem.current.currentInputModule != null) {
				selectedGoBefore = EventSystem.current.currentSelectedGameObject;
				EventSystem.current.currentInputModule.DeactivateModule ();
				EventSystem.current.currentInputModule.enabled = false;
			}
		}

	}

	void OnDisable ()
	{
		if (zeroTimeScale)
			Time.timeScale = timeScaleBefore;
		
		if (disableUiInput) {
			if (EventSystem.current != null && EventSystem.current.currentInputModule != null) {
				EventSystem.current.currentInputModule.enabled = true;
				EventSystem.current.currentInputModule.ActivateModule ();
				EventSystem.current.SetSelectedGameObject (selectedGoBefore);
			}
		}
	}
}
