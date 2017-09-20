using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class ScreenshotEditorCapturer : ScriptableObject
{
	private const string ScreenShotsFolderPath = "Assets/ScreenShots/";

	[MenuItem ("Custom/ScreenShot Transparent", false, 100)]
	public static void SaveScreenShotTransparent ()
	{
		SaveTransparentScreenShot ();
		Debug.Log ("Save transaprent screenshot.");
	}

	private static void SaveTransparentScreenShot ()
	{
		Camera.main.backgroundColor = Color.clear;
		Camera.main.clearFlags = CameraClearFlags.Color;

		Texture2D screenshot = new Texture2D (Screen.width - 0, Screen.height - 0);
		screenshot.ReadPixels (new Rect (0, 0, Screen.width - 0, Screen.height - 0), 0, 0);
		screenshot.Apply ();
		
		byte[] bytes = screenshot.EncodeToPNG ();
		DestroyImmediate (screenshot);
		
		string filePath = Application.dataPath + (ScreenShotsFolderPath + GenerateScreenShotFileNameByTime ()).Substring (6);
		System.IO.File.WriteAllBytes (filePath, bytes);
	}

	[MenuItem ("Custom/ScreenShot %#t", false, 101)]
	public static void SaveScreenShotSize1 ()
	{
		SaveScreenShot (1);
		Debug.Log ("Save screenshot.");
	}

	[MenuItem ("Custom/ScreenShot x2", false, 102)]
	public static void SaveScreenShotSize2 ()
	{
		SaveScreenShot (2);
		Debug.Log ("Save screenshot x2.");
	}

	[MenuItem ("Custom/ScreenShot x3", false, 103)]
	public static void SaveScreenShotSize3 ()
	{
		SaveScreenShot (3);
		Debug.Log ("Save screenshot x3.");
	}

	[MenuItem ("Custom/ScreenShot x5", false, 104)]
	public static void SaveScreenShotSize5 ()
	{
		SaveScreenShot (5);
		Debug.Log ("Save screenshot x5.");
	}

	[MenuItem ("Custom/ScreenShot x8", false, 105)]
	public static void SaveScreenShotSize8 ()
	{
		SaveScreenShot (8);
		Debug.Log ("Save screenshot x8.");
	}

	public static void SaveScreenShot (int superSize)
	{
		SaveScreenShot (superSize, GenerateScreenShotFileNameByTime ());
	}

	public static void SaveScreenShot (int superSize, string fileName)
	{
		string filePath = ScreenShotsFolderPath + fileName;
		SaveAssetUtility.CreateFolderForAssetIfNeeded (filePath);
		ScreenCapture.CaptureScreenshot (filePath, superSize);
		AssetDatabase.Refresh ();
	}

	private static string GenerateScreenShotFileNameByTime ()
	{
		DateTime now = DateTime.Now;
		string fileName = string.Format ("Screenshot_{0}-{1:00}-{2:00}_{3:00}-{4:00}-{5:00}.png",
			                  now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
		
		return fileName;
	}
}
