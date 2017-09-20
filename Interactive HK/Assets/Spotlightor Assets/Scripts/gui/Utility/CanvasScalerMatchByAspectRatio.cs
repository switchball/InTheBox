using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(CanvasScaler))]
public class CanvasScalerMatchByAspectRatio : MonoBehaviour
{
	public AnimationCurve matchWidthOrHeightByAspectRatio = new AnimationCurve (
		                                                        new Keyframe (9f / 16f, 0f), 
		                                                        new Keyframe (2f / 3f, 0f), 
		                                                        new Keyframe (3f / 4f, 1f)
	                                                        );

	void Awake ()
	{
		UpdateCanvasScalerSetting ();
	}

	void Update ()
	{
		UpdateCanvasScalerSetting ();
	}

	public void UpdateCanvasScalerSetting ()
	{
		float aspectRatio = (float)Screen.width / (float)Screen.height;
		float matchWidthOrHeight = matchWidthOrHeightByAspectRatio.Evaluate (aspectRatio);

		CanvasScaler scaler = GetComponent<CanvasScaler> ();
		if (scaler.matchWidthOrHeight != matchWidthOrHeight)
			scaler.matchWidthOrHeight = matchWidthOrHeight;
	}

	void Reset ()
	{
		GetComponent<CanvasScaler> ().screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
	}
}
