using UnityEngine;
using UnityEditor;
using System.Collections;

public class ProjectAssetsFolderStructureCreator : ScriptableObject
{
	public static string ProjectAssetsFolderPath {
		get { return string.Format ("Assets/_{0} Project Assets", PlayerSettings.productName);}
	}
	
	public static string[] AssetFolderNames = new string[]{
		"3D Arts", "3D Arts/Props", "3D Arts/Environment", "3D Arts/Actors",
		"Animations", "Editor", 
		"Fonts", "Materials", 
		"Prefabs", "Prefabs/Actors", "Prefabs/Props", "Prefabs/Environment", "Prefabs/VFX", "Prefabs/GUI", "Prefabs/Misc",
		"Resources", "Scenes", 
		"Scripts", "Scripts/Generic", "Scripts/Modules", "Scripts/Scenes", "Scripts/Editor", "Scripts/_DebugDev",
		"Shaders", 
		"Audio", "Audio/Music", "Audio/SFX", "Audio/Ambient",
		"Textures", "Textures/App", "Textures/VFX", "Textures/GUI",
		"ScriptableObjects"
	};
	
	[MenuItem("Spotlightor/Project/Create Folder Structure")]
	public static void CreateProjectAssetsFolderStructure ()
	{
		string rootDir = string.Format ("{0}/_{1} Project Assets", Application.dataPath, PlayerSettings.productName);
		foreach (string assetFolderName in AssetFolderNames) {
			System.IO.Directory.CreateDirectory (string.Format ("{0}/{1}", rootDir, assetFolderName));
		}
		
		AssetDatabase.Refresh (ImportAssetOptions.Default);
	}
}
