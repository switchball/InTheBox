using UnityEngine;
using System.Collections;

public class HideByRefProperty : HideByProperty
{
	public bool hideIfRefNotNull = true;

	public HideByRefProperty (string propertyName, bool hideIfRefNotNull) : base (propertyName)
	{
		this.hideIfRefNotNull = hideIfRefNotNull;
	}
}
