using UnityEngine;
using System.Collections;

public class MeshTexturesPacker : ScriptableObject
{
	public const string PackedAssetsFolderName = "Packed";
	public string atlasName = "atlas";
	public int textureSize = 1024;
	public int padding = 0;
	public bool discardAlpha = false;
	public Vector3 verticesOffset = Vector3.zero;
	public GameObject[] meshRoots;
}
