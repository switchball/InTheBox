using UnityEngine;
using System.Collections;

public static class ColorHexConverter
{
	public static string ColorToHex (Color32 color)
	{
		string hex = "#" + color.r.ToString ("x2") + color.g.ToString ("x2") + color.b.ToString ("x2") + color.a.ToString ("x2");
		return hex;
	}
 
	public static Color HexToColor (string hex)
	{
		if (hex [0] == '#')
			hex = hex.Substring (1);
		
		byte r = byte.Parse (hex.Substring (0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse (hex.Substring (2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse (hex.Substring (4, 2), System.Globalization.NumberStyles.HexNumber);
		return new Color32 (r, g, b, 255);
	}
}
