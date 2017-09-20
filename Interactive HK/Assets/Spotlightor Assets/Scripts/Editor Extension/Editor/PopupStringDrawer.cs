using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(PopupString))]
public class PopupStringDrawer : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		PopupString popupString = attribute as PopupString;

		if (property.propertyType == SerializedPropertyType.String) {
			string stringValue = property.stringValue;
			int selectedIndex = popupString.options.IndexOf (stringValue);
			selectedIndex = EditorGUI.Popup (position, label.text, selectedIndex, popupString.options.ToArray ());
			if (selectedIndex >= 0)
				property.stringValue = popupString.options [selectedIndex];
			else
				property.stringValue = "";
		} else
			EditorGUI.LabelField (position, label.text, "Use PopupString with string.");
	}
}
