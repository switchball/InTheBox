using UnityEngine;
using UnityEditor;
using System.Collections;

public abstract class HideByPropertyDrawer : PropertyDrawer
{
	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return IsHidden (property) ? 0 : base.GetPropertyHeight (property, label);
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		if (!IsHidden (property))
			EditorGUI.PropertyField (position, property, label);
	}

	private bool IsHidden (SerializedProperty property)
	{
		HideByProperty hideAttribute = attribute as HideByProperty;
		SerializedProperty hiderProperty = property.serializedObject.FindProperty (hideAttribute.propertyName);
		return IsHiddenBy (hiderProperty);
	}

	protected abstract bool IsHiddenBy (SerializedProperty hiderProperty);
}
