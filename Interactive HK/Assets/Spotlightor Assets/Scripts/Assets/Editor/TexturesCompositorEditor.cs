using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TexturesCompositor))]
public class TexturesCompositorEditor : Editor
{
	private TexturesCompositor Compositor{ get { return target as TexturesCompositor; } }

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		if (GUILayout.Button ("Composite")) {
			Texture2D compositeTexture = Compositor.Composite ();

			string path = AssetDatabase.GetAssetPath (Compositor);
			string folderPath = path.Substring (0, path.LastIndexOf ("/") + 1);
			string texturePath = folderPath + Compositor.name.ToLower () + ".png";
			
			SaveAssetUtility.SaveTexture (compositeTexture, texturePath);
		}
	}
}
