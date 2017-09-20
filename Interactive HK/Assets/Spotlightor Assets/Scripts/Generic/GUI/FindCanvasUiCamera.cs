using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class FindCanvasUiCamera : MonoBehaviour
{
	public string uiCameraTag = "UI Camera";

	IEnumerator Start ()
	{
		GameObject uiCameraGo = null;
		do {
			uiCameraGo = GameObject.FindGameObjectWithTag (uiCameraTag);
			yield return null;
		} while(uiCameraGo == null);

		if (uiCameraGo != null && uiCameraGo.GetComponent<Camera> () != null)
			GetComponent<Canvas> ().worldCamera = uiCameraGo.GetComponent<Camera> ();
		else
			this.LogWarning ("Cannot find camera with tag: {0}", uiCameraTag);

		Destroy (this);
	}
}
