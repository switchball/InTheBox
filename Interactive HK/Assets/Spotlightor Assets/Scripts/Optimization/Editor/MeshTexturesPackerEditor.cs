using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

[CustomEditor (typeof(MeshTexturesPacker))]
public class MeshTexturesPackerEditor : Editor
{
	public MeshTexturesPacker Packer{ get { return target as MeshTexturesPacker; } }

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		if (GUILayout.Button ("Pack"))
			Pack ();
	}

	public void Pack ()
	{
		string path = AssetDatabase.GetAssetPath (Packer);
		
		string meshFolderPath = path.Substring (0, path.LastIndexOf ("/")) + "/" + MeshTexturesPacker.PackedAssetsFolderName;
		
		if (Packer.meshRoots.Length > 0) {
			List<Texture2D> texturesToPack = new List<Texture2D> ();
			foreach (GameObject go in Packer.meshRoots) {
				MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer> (true);
				foreach (MeshRenderer rd in renderers) {
					Texture2D tex = rd.sharedMaterial.GetTexture ("_MainTex") as Texture2D;
					if (tex != null && !texturesToPack.Contains (tex))
						texturesToPack.Add (tex);
				}
			}
			
			if (texturesToPack.Count > 0) {
				Texture2D atlas = new Texture2D (Packer.textureSize, Packer.textureSize);
				Rect[] atlasTextureRects = atlas.PackTextures (texturesToPack.ToArray (), Packer.padding, Packer.textureSize, false);
				
				if (Packer.discardAlpha) {
					Color32[] colors = atlas.GetPixels32 ();
					for (int i = 0; i < colors.Length; i++)
						colors [i].a = 255;
					atlas.SetPixels32 (colors);
					atlas.Apply ();
				}
				
				string atlasTexturePath = meshFolderPath + "/" + Packer.atlasName + ".png";
				Texture2D atlasTextureAsset = SaveAssetUtility.SaveTexture (atlas, atlasTexturePath);
				
				if (atlasTextureAsset != null) {
					foreach (GameObject go in Packer.meshRoots) {
						List<MeshFilter> childMeshFilters = new List<MeshFilter> (go.GetComponentsInChildren<MeshFilter> (true));
						
						foreach (MeshFilter mf in childMeshFilters) {
							Texture2D tex = mf.GetComponent<MeshRenderer> ().sharedMaterial.mainTexture as Texture2D;
							if (tex != null) {
								int texIndex = texturesToPack.IndexOf (tex);
								Rect newUvSpace = atlasTextureRects [texIndex];
								
								Mesh atlasMesh = new Mesh ();
								Mesh originalMesh = mf.sharedMesh;
								
								Vector2[] uv = new Vector2[originalMesh.uv.Length];
								for (int i = 0; i < originalMesh.uv.Length; i++) {
									Vector2 vertexUv = new Vector2 (
										                   newUvSpace.x + newUvSpace.width * originalMesh.uv [i].x,
										                   newUvSpace.y + newUvSpace.height * originalMesh.uv [i].y
									                   );
									uv [i] = vertexUv;
								}
								
								Vector2[] uv2 = new Vector2[originalMesh.uv2.Length];
								for (int i = 0; i < originalMesh.uv2.Length; i++)
									uv2 [i] = new Vector2 (originalMesh.uv2 [i].x, originalMesh.uv2 [i].y);
								
								int[] triangles = new int[originalMesh.triangles.Length];
								Array.Copy (originalMesh.triangles, triangles, triangles.Length);
								
								Vector3[] vertices = new Vector3[originalMesh.vertices.Length];
								Array.Copy (originalMesh.vertices, vertices, vertices.Length);
								if (Packer.verticesOffset != Vector3.zero) {
									for (int i = 0; i < vertices.Length; i++)
										vertices [i] += Packer.verticesOffset;
								}
								
								Vector3[] normals = new Vector3[originalMesh.normals.Length];
								Array.Copy (originalMesh.normals, normals, normals.Length);
								
								Vector4[] tangents = new Vector4[originalMesh.tangents.Length];
								Array.Copy (originalMesh.tangents, tangents, tangents.Length);
								
								atlasMesh.vertices = vertices;
								atlasMesh.triangles = triangles;
								atlasMesh.normals = normals;
								atlasMesh.tangents = tangents;
								atlasMesh.uv = uv;
								atlasMesh.uv2 = uv2;
								atlasMesh.RecalculateBounds ();
								
								Debug.Log (string.Format ("Mesh Pack Result = vertices:{0}/{1} uv:{2}/{3} triangles:{4}/{5} tangents:{6}/{7} normals:{8}/{9} uv2:{10}/{11}", 
									atlasMesh.vertices.Length, originalMesh.vertices.Length,
									atlasMesh.uv.Length, originalMesh.uv.Length,
									atlasMesh.triangles.Length, originalMesh.triangles.Length,
									atlasMesh.tangents.Length, originalMesh.tangents.Length,
									atlasMesh.normals.Length, originalMesh.normals.Length,
									atlasMesh.uv2.Length, originalMesh.uv2.Length
								));

								string meshAssetPath = mf.sharedMesh.name + ".asset";
								if (childMeshFilters.Count > 1)
									meshAssetPath = string.Format ("{0}/{1}/{2}", meshFolderPath, go.name, meshAssetPath);
								else
									meshAssetPath = string.Format ("{0}/{1}", meshFolderPath, meshAssetPath);
								
								SaveAssetUtility.SaveMesh (atlasMesh, meshAssetPath);
							}
						}
						
					} 
				}
			} else {
				Debug.LogWarning ("Nothing to pack");
			}
		}
	}
}
