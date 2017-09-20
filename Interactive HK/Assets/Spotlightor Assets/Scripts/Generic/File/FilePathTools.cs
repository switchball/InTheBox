using UnityEngine;
using System.Collections;
using System.IO;

public static class FilePathTools
{
	public static string GetFolderPathInDocumments (string folderName)
	{
		string myDocumentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		
		bool isOSX = Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXDashboardPlayer;
		
		string folderSplitter = isOSX ? "//" : "\\";

		string folderPath = myDocumentsPath + folderSplitter + folderName + folderSplitter;

		if (!Directory.Exists (folderPath)) {
			Directory.CreateDirectory (folderPath);
			Debug.Log (string.Format ("Folder created at path: {0}", folderPath));
		}
		return folderPath;
	}
}
