using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(SingleLineLabel))]
public class SingleLineLabelDrawer : PropertyDrawer
{
	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return base.GetPropertyHeight (property, label) * 2;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		Rect labelPosition = position;
		labelPosition.height *= 0.5f;

		EditorGUI.LabelField (labelPosition, label);

		Rect propertyPosition = position;
		propertyPosition.height *= 0.5f;
		propertyPosition.y = labelPosition.yMax;

		EditorGUI.PropertyField (propertyPosition, property, GUIContent.none);
	}
}
