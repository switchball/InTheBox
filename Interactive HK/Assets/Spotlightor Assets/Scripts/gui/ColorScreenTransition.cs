using UnityEngine;
using System.Collections;

public class ColorScreenTransition : ValueTransitionManager
{
	public Color color = Color.black;
	private Texture2D whiteTexture;

	public Texture WhiteTexture {
		get {
			if (whiteTexture == null)
				whiteTexture = Texture2D.whiteTexture;
			return whiteTexture;
		}
	}

	protected override void OnProgressValueUpdated (float progress)
	{
		
	}

	void OnGUI ()
	{
        if (ProgressValue != 0) {
			GUI.color = new Color (color.r, color.g, color.b, ProgressValue * color.a);
			GUI.depth = -1;
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), WhiteTexture);
		}
	}
}
