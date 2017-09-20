using UnityEngine;
using System.Collections;

public class HideByBooleanProperty : HideByProperty
{
	public bool hideIfTrue = true;

	public HideByBooleanProperty (string propertyname, bool hideIfPropertyTrue) : base (propertyname)
	{
		this.hideIfTrue = hideIfPropertyTrue;
	}
}
