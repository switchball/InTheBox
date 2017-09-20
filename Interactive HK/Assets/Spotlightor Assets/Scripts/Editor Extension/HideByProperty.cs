using UnityEngine;
using System.Collections;

public abstract class HideByProperty : PropertyAttribute
{
	public string propertyName;

	public HideByProperty (string propertyName)
	{
		this.propertyName = propertyName;
	}
}
