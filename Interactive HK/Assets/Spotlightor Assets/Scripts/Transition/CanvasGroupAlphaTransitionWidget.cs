using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupAlphaTransitionWidget : TransitionWidget
{

	public override void UpdateTransitionProgress (float progress)
	{
		GetComponent<CanvasGroup> ().alpha = progress;
	}
}
