using UnityEngine;
using System.Collections;

#if UNITY_XBOXONE
using Users;
#endif

public class XboxUserTextPropertyDisplayer : ContentTextPropertyDisplayer<XboxUser>
{
	public enum PropertyTypes
	{
		DisplayName,
	}

	public PropertyTypes propertyType = PropertyTypes.DisplayName;
	public int maxLength = 20;

	protected override string GetTextToDisplay (XboxUser xboxUser)
	{
		string text = "";
		if (propertyType == PropertyTypes.DisplayName)
			text = xboxUser.DisplayName;

		if (text.Length > maxLength)
			text = text.Substring (0, maxLength) + "...";

		return text;
	}

}
