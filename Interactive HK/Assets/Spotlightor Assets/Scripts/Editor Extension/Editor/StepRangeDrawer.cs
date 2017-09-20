using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer (typeof(StepRange))]
public class StepRangeDrawer : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		StepRange stepSlider = attribute as StepRange;

		float newValue = EditorGUI.Slider (position, label, property.floatValue, stepSlider.min, stepSlider.max);
		int step = Mathf.RoundToInt ((newValue - stepSlider.min) / stepSlider.step);
		float stepValue = (float)step * stepSlider.step + stepSlider.min;
		if (newValue != stepValue)
			property.floatValue = stepValue;
	}
}
