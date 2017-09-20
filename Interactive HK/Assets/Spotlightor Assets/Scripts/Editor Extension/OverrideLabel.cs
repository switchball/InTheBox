using UnityEngine;
using System.Collections;

public class OverrideLabel : PropertyAttribute
{
	public string label;
	
	public OverrideLabel (string label)
	{
		this.label = label;
	}
}
