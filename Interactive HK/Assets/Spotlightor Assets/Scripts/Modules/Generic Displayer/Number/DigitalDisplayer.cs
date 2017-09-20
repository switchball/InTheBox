using UnityEngine;
using System.Collections;

public class DigitalDisplayer : NumberTextDisplayer
{
	public enum FloatToIntMethods
	{
		Floor,
		Ceil,
		Round,
	}
	public int digitsCount = 3;
	public int maxValue = 0;
	public FloatToIntMethods floatToIntMethod = FloatToIntMethods.Round;
	private string stringFormat = "";

	protected override string FormatNumberValueToString (float numberValue)
	{
		if (stringFormat == "") {
			if (digitsCount > 0) {
				stringFormat = "{0:";
				for (int i = 0; i < digitsCount; i++)
					stringFormat += "0";
				stringFormat += "}";
			} else
				stringFormat = "{0}";
		}
		if (maxValue != 0)
			numberValue = Mathf.Min (numberValue, maxValue);
		return string.Format (stringFormat, ConverFloatToInt (numberValue));
	}

	private int ConverFloatToInt (float numberValue)
	{
		if (floatToIntMethod == FloatToIntMethods.Ceil)
			return Mathf.CeilToInt (numberValue);
		else if (floatToIntMethod == FloatToIntMethods.Floor)
			return Mathf.FloorToInt (numberValue);
		else
			return Mathf.RoundToInt (numberValue);
	}
}
