using UnityEngine;
using System.Collections;

public class RectTransformShakeVisualizer : ShakeVisualizer
{
	public float xyIntensityToOffset = 10f;
	public float zIntensityToScale = -0.1f;
	private Vector2 defaultAnchoredPosition = Vector2.zero;
	
	void Awake ()
	{
		ResetDefaultAnchoredPosition ();
	}

	public void ResetDefaultAnchoredPosition ()
	{
		defaultAnchoredPosition = GetComponent<RectTransform> ().anchoredPosition;
	}
	
	protected override void VisualizeShake (Vector3 intensity)
	{
		Vector2 anchoredPosition = new Vector2 (intensity.x, intensity.y) * xyIntensityToOffset;
		anchoredPosition += defaultAnchoredPosition;
		GetComponent<RectTransform> ().anchoredPosition = anchoredPosition;

		transform.SetUniformLocalScale (1 + intensity.z * zIntensityToScale);
	}

}
