using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(ClampAttribute))]
public class ClampAttributePropertyDrawer : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		ClampAttribute clamp = attribute as ClampAttribute;

		EditorGUI.PropertyField (position, property, label);

		if (property.propertyType == SerializedPropertyType.Float) {
			property.floatValue = Mathf.Clamp (property.floatValue, clamp.min, clamp.max);
		} else if (property.propertyType == SerializedPropertyType.Integer) {
			property.intValue = Mathf.Clamp (property.intValue, (int)clamp.min, (int)clamp.max);
		}
	}
}
