using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer (typeof(HideByBooleanProperty))]
public class HideByBooleanPropertyDrawer : HideByPropertyDrawer
{
	protected override bool IsHiddenBy (SerializedProperty hiderProperty)
	{
		return (attribute as HideByBooleanProperty).hideIfTrue == hiderProperty.boolValue;
	}
}
