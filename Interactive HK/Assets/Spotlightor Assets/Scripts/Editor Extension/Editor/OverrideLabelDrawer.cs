using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(OverrideLabel))]
public class OverrideLabelDrawer : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		OverrideLabel overrideLabel = attribute as OverrideLabel;

		EditorGUI.PropertyField (position, property, new GUIContent(overrideLabel.label));
	}
}
