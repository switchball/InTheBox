using UnityEngine;
using System.Collections;
using System;

public class ScreenshotCommand : CoroutineCommandBehavior
{
	private const string ScreenShotsFolderPath = "ScreenShots/";

	public float delay = 0;
	public string screenshotName = "";
	public int superSize = 1;

	protected override IEnumerator CoroutineCommand ()
	{
		yield return new WaitForSeconds (delay);

		Screenshot ();
	}

	protected void Screenshot ()
	{
		#if UNITY_EDITOR
		string fileName = screenshotName;
		if (string.IsNullOrEmpty (fileName)) {
			DateTime now = DateTime.Now;
			fileName = string.Format ("Screenshot_{0}-{1:00}-{2:00}_{3:00}-{4:00}-{5:00}",
				now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
		}
		string filePath = ScreenShotsFolderPath + fileName + ".png";
		ScreenCapture.CaptureScreenshot (filePath, superSize);
		#endif
	}
}
