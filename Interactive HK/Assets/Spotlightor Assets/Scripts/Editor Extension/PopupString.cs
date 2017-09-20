using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupString : PropertyAttribute
{
	public List<string> options;

	public PopupString (string[] choiceStrings)
	{
		this.options = new List<string>(choiceStrings);
	}
}
