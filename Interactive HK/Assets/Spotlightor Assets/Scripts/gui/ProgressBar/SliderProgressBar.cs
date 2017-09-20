using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Slider))]
public class SliderProgressBar : ProgressBar
{
	protected override void UpdateProgressDisplay (float progress)
	{
		Slider slider = GetComponent<Slider> ();
		slider.value = Mathf.Lerp (slider.minValue, slider.maxValue, progress);
	}
}
