using UnityEngine;
using UnityEditor;
using System.Collections;

public class PostSwapTextureChannels : AssetPostprocessor
{
	public const string TargetTexturePostfix = "_";

	void OnPostprocessTexture (Texture2D texture)
	{
		string fileName = System.IO.Path.GetFileNameWithoutExtension (assetPath);
		if (fileName.Substring (fileName.Length - TargetTexturePostfix.Length, TargetTexturePostfix.Length) == TargetTexturePostfix) {
			Color32[] colors = texture.GetPixels32 ();
			for (int i = 0; i < colors.Length; i++) {
				Color32 c = colors [i];
				int grayRgb = (c.r + c.g + c.b) / 3;
				c.r = c.a;
				c.g = c.a;
				c.b = c.a;

				c.a = (byte)grayRgb;

				colors [i] = c;
			}

			texture.SetPixels32 (colors);
			texture.Apply (true);
			Debug.Log (string.Format ("Texture {0} RGB <-> A switched.", fileName));
		}
	}
}
