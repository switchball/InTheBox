using UnityEngine;
using System.Collections;

public class TexturesCompositor : ScriptableObject
{
	public Texture2D rgb;
	public Texture2D alpha;

	public Texture2D Composite ()
	{
		Color32[] rgbColors = rgb.GetPixels32 ();
		Color32[] alphaColors = alpha.GetPixels32 ();
		Color32[] resultColors = new Color32[rgbColors.Length];
		for (int i= 0; i < resultColors.Length; i++) {
			Color32 rgbColor = rgbColors [i];
			byte a = alphaColors [i].a;
			resultColors [i] = new Color32 (rgbColor.r, rgbColor.g, rgbColor.b, a);
		}

		Texture2D result = new Texture2D (rgb.width, rgb.height);
		result.SetPixels32 (resultColors);
		result.Apply ();
		return result;
	}
}
