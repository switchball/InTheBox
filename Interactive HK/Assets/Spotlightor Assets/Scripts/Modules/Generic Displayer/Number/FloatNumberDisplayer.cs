using UnityEngine;
using System.Collections;

public class FloatNumberDisplayer : NumberTextDisplayer
{
	public int floatPrecision = 1;

	protected override string FormatNumberValueToString (float numberValue)
	{
		string format = "{0:0.";
		for (int i = 0; i < floatPrecision; i++)
			format += "0";
		format += "}";
		return string.Format (format, numberValue);
	}
}
