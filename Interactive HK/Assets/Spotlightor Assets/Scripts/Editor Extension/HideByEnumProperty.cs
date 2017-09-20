using UnityEngine;
using System.Collections;

public class HideByEnumProperty : HideByProperty
{
	public int targetEnumValueIndex = 0;
	public bool hideIfEqualIndex = true;

	public HideByEnumProperty (string propertyname, int enumIndex, bool hideIfEqualIndex) : base (propertyname)
	{
		this.targetEnumValueIndex = enumIndex;
		this.hideIfEqualIndex = hideIfEqualIndex;
	}
}
