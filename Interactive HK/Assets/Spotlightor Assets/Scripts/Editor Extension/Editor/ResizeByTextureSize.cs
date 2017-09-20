using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

public class ResizeByTextureSize : ScriptableObject
{
	[MenuItem("Spotlightor/GUI/Resize GUITexture by Texture Size")]
	public static void ResizeGUITextureByTextureSize ()
	{
		GameObject[] selectedObjects = Selection.gameObjects;
		foreach (GameObject go in selectedObjects) {
			GUITexture goGUITexture = go.GetComponent<GUITexture> ();
			if (goGUITexture) {
				Rect newInest = goGUITexture.pixelInset;
				newInest.width = goGUITexture.texture.width;
				newInest.height = goGUITexture.texture.height;
				goGUITexture.pixelInset = newInest;
			}
		}
	}

	[MenuItem("Spotlightor/GUI/Resize Quad by Texture Size + Default Resolution")]
	public static void ResizePlane1x1ByTextureSizeAndDefaultResolution ()
	{
		GameObject[] selectedObjects = Selection.gameObjects;
		foreach (GameObject go in selectedObjects) {
			if (go.GetComponent<Renderer>() != null) {
				Vector3 newScale = Vector3.one;
				Texture texture = go.GetComponent<Renderer>().sharedMaterial.mainTexture;
				newScale.x = texture.width * 0.001f;
				newScale.y = texture.height * 0.001f;
				go.transform.localScale = newScale;
			}
		}
	}
}
