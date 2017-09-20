using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class ImageFillProgressBar : ProgressBar
{
	protected override void UpdateProgressDisplay (float progress)
	{
		GetComponent<Image> ().fillAmount = progress;
	}
}
