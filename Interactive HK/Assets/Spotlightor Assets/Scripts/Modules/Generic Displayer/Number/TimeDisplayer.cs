using UnityEngine;
using System.Collections;

public class TimeDisplayer : NumberTextDisplayer
{
	private const string TextFormatTokenMinutes = "[m]";
	private const string TextFormatTokenSeconds = "[s]";
	private const string TextFormatTokenMilliseconds = "[ms]";
	[Header("Avaialbe tokens: [m] [s] [ms]")]
	public string
		timeFormat = "[m]'[s]\"[ms]";
	
	protected override string FormatNumberValueToString (float numberValue)
	{
		string timeText = timeFormat;
		
		if (timeText.Contains (TextFormatTokenMinutes)) {
			int minutes = Mathf.FloorToInt (numberValue / 60);
			timeText = timeText.Replace (TextFormatTokenMinutes, string.Format ("{0:00}", minutes));
		}
		
		if (timeText.Contains (TextFormatTokenSeconds)) {
			int seconds = Mathf.FloorToInt (numberValue) % 60;
			timeText = timeText.Replace (TextFormatTokenSeconds, string.Format ("{0:00}", seconds));
		}
		
		if (timeText.Contains (TextFormatTokenMilliseconds)) {
			int milliseconds = Mathf.FloorToInt ((numberValue - Mathf.FloorToInt (numberValue)) * 1000);
			timeText = timeText.Replace (TextFormatTokenMilliseconds, string.Format ("{0:000}", milliseconds));
		}
		
		return timeText;
	}
}
