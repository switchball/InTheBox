using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer (typeof(HideByEnumProperty))]
public class HideByEnumPropertyDrawer : HideByPropertyDrawer
{
	protected override bool IsHiddenBy (SerializedProperty hiderProperty)
	{
		HideByEnumProperty enumHiderAttribute = attribute as HideByEnumProperty;
		bool equalIndex = enumHiderAttribute.targetEnumValueIndex == hiderProperty.enumValueIndex;
		return enumHiderAttribute.hideIfEqualIndex == equalIndex;
	}
}
