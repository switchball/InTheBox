using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Editor script to create ScriptableObject asset.
/// </summary>
public class ScriptableObjectCreator : ScriptableObject {
	
	[MenuItem("Assets/Create ScriptableObject Asset")]
	public static void CreateScriptableObjectAsset(){
		foreach(Object obj in Selection.objects){
			if(obj is MonoScript){
				MonoScript script = obj as MonoScript;
				Object scriptableAsset = ScriptableObject.CreateInstance(script.GetClass());

				string scriptPath = AssetDatabase.GetAssetPath (obj);
				string scriptFolderPath = scriptPath.Substring (0, scriptPath.LastIndexOf ("/"));
				string scriptableAssetPath = scriptFolderPath + "/" + script.name + ".asset";

				AssetDatabase.CreateAsset(scriptableAsset, scriptableAssetPath);
			}
		}
	}
}
