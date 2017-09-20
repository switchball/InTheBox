using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TextureAtlasMeshGeneratorMenuScript : ScriptableObject
{
	[MenuItem("Spotlightor/GUI/Reset Scale by Atlas")]
	public static void ResetScale ()
	{
		List<MeshFilter> meshFilters = new List<MeshFilter> ();
		foreach (GameObject selectedGo in Selection.gameObjects) {
			MeshFilter mf = selectedGo.GetComponent<MeshFilter> ();
			if (mf != null)
				meshFilters.Add (mf);
		}

		foreach (MeshFilter selectedMeshFilter in meshFilters) {
			if (selectedMeshFilter != null & selectedMeshFilter.sharedMesh != null) {
				Mesh selectedMesh = selectedMeshFilter.sharedMesh;
			
				string meshPath = AssetDatabase.GetAssetPath (selectedMesh);
			
				string meshGeneratorFolderPath = meshPath.Substring (0, meshPath.LastIndexOf ("/"));
				meshGeneratorFolderPath = meshGeneratorFolderPath.Substring (0, meshGeneratorFolderPath.LastIndexOf ("/")) + "/";
			
				string meshGeneratorFolderDirectory = Application.dataPath.Substring (0, Application.dataPath.LastIndexOf ("/")) + "/" + meshGeneratorFolderPath;

				string[] assetFileGlobalPaths = Directory.GetFiles (meshGeneratorFolderDirectory, "*.asset", SearchOption.TopDirectoryOnly);
			
				foreach (string assetFileGlobalPath in assetFileGlobalPaths) {
					string assetFilePath = assetFileGlobalPath.Substring (assetFileGlobalPath.IndexOf ("Assets/"));
				
					TextureAtlasMeshGeneartor meshGenerator = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(TextureAtlasMeshGeneartor)) as TextureAtlasMeshGeneartor;
					if (meshGenerator != null) {
					
						foreach (Texture2D t in meshGenerator.atlas.textures) {
							if (t.name == selectedMesh.name) {
								Debug.Log ("Find in atlas textures");
								selectedMeshFilter.transform.localScale = new Vector3 (t.width * 0.001f, t.height * 0.001f, 1);
							}
						}
					
						foreach (TextureAtlasMeshGeneartor.SlicedSpriteMeshSetting slicedMeshSetting in meshGenerator.slicedSprites) {
							List<string> textureNames = new List<string> ();
							textureNames.Add (slicedMeshSetting.texture.name);
							foreach (Texture t in slicedMeshSetting.moreTextures)
								textureNames.Add (t.name);
							foreach (TextureAtlasMeshGeneartor.SlicedSpriteMeshSetting.SpriteInstanceSetting instanceSetting in slicedMeshSetting.instances) {
								foreach (string textureName in textureNames) {
									string meshName = slicedMeshSetting.GetMeshNameForInstanceSetting (textureName, instanceSetting);
									if (meshName == selectedMesh.name) {
										Debug.Log ("Find in sliced sprites");
										selectedMeshFilter.transform.localScale = new Vector3 (instanceSetting.width * 0.001f, instanceSetting.height * 0.001f, 1);
									}
								}
							}
						}
					
						break;
					}
				}
			}
#if UNITY_5
			EditorUtility.UnloadUnusedAssetsImmediate ();
#endif
			#if !UNITY_5
			EditorUtility.UnloadUnusedAssets ();
			#endif
		}
	}

	[MenuItem("Spotlightor/GUI/Update Sliced Sprite Setting")]
	public static void UpdateSlicedSpriteSetting ()
	{
		MeshFilter selectedMeshFilter = Selection.activeGameObject.GetComponent<MeshFilter> ();
		if (selectedMeshFilter != null && selectedMeshFilter.sharedMesh != null) {
			Mesh selectedMesh = selectedMeshFilter.sharedMesh;
			string meshPath = AssetDatabase.GetAssetPath (selectedMesh);
			
			string meshGeneratorFolderPath = meshPath.Substring (0, meshPath.LastIndexOf ("/"));
			meshGeneratorFolderPath = meshGeneratorFolderPath.Substring (0, meshGeneratorFolderPath.LastIndexOf ("/")) + "/";
			
			string meshGeneratorFolderDirectory = Application.dataPath.Substring (0, Application.dataPath.LastIndexOf ("/")) + "/" + meshGeneratorFolderPath;
			
			string[] assetFileGlobalPaths = Directory.GetFiles (meshGeneratorFolderDirectory, "*.asset", SearchOption.TopDirectoryOnly);
			
			foreach (string assetFileGlobalPath in assetFileGlobalPaths) {
				string assetFilePath = assetFileGlobalPath.Substring (assetFileGlobalPath.IndexOf ("Assets/"));
				
				TextureAtlasMeshGeneartor meshGenerator = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(TextureAtlasMeshGeneartor)) as TextureAtlasMeshGeneartor;
				if (meshGenerator != null) {
					foreach (TextureAtlasMeshGeneartor.SlicedSpriteMeshSetting slicedMeshSetting in meshGenerator.slicedSprites) {
						List<string> textureNames = new List<string> ();
						textureNames.Add (slicedMeshSetting.texture.name);
						foreach (Texture t in slicedMeshSetting.moreTextures)
							textureNames.Add (t.name);
						foreach (TextureAtlasMeshGeneartor.SlicedSpriteMeshSetting.SpriteInstanceSetting instanceSetting in slicedMeshSetting.instances) {
							foreach (string textureName in textureNames) {
								string meshName = slicedMeshSetting.GetMeshNameForInstanceSetting (textureName, instanceSetting);
								if (meshName == selectedMesh.name) {
									instanceSetting.width = Mathf.RoundToInt (selectedMeshFilter.transform.localScale.x * 1000);
									instanceSetting.height = Mathf.RoundToInt (selectedMeshFilter.transform.localScale.y * 1000);
									EditorUtility.SetDirty (meshGenerator);
									Debug.Log ("Setting updated");
									return;
								}
							}
						}
					}
					break;
				}
			}
		}
	}
}
