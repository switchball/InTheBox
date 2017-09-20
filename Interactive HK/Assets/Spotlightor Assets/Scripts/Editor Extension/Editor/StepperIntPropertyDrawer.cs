using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer (typeof(StepperInt))]
public class StepperIntPropertyDrawer : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		StepperInt stepperInt = attribute as StepperInt;

		EditorGUI.BeginProperty (position, label, property);
		EditorGUI.BeginChangeCheck ();
		int newValue = property.intValue;

		Rect labelRect = EditorGUI.PrefixLabel (position, label);

		Rect amountRect = new Rect (labelRect.x, position.y, 40, position.height);
		newValue = EditorGUI.IntField (amountRect, property.intValue);

		Rect addRect = new Rect (amountRect.xMax + 5, position.y, 30, position.height);
		if (GUI.Button (addRect, "+"))
			newValue += stepperInt.step;
		
		Rect minusRect = new Rect (addRect.xMax + 5, position.y, 30, position.height);
		if (GUI.Button (minusRect, "-"))
			newValue -= stepperInt.step;

		if (EditorGUI.EndChangeCheck ())
			property.intValue = Mathf.Clamp (newValue, stepperInt.min, stepperInt.max);
		
		EditorGUI.EndProperty ();
	}
}
