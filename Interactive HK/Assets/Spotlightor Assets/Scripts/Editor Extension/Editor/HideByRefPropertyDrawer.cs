using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer (typeof(HideByRefProperty))]
public class HideByRefPropertyDrawer : HideByPropertyDrawer
{
	protected override bool IsHiddenBy (SerializedProperty hiderProperty)
	{
		return (hiderProperty.objectReferenceValue != null) == (attribute as HideByRefProperty).hideIfRefNotNull;
	}
}
