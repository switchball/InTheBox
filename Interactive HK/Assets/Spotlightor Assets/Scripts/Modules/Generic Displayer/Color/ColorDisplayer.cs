using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorDisplayer : MonoBehaviour
{
	public bool tintSharedMaterial = false;
	public string materialPropertyName = "_Color";

	private delegate void DisplayColorDelegate (Color color);

	private DisplayColorDelegate displayColor;

	private DisplayColorDelegate DisplayColor {
		get {
			if (displayColor == null) {
				if (GetComponent<Graphic> () != null)
					displayColor = UpdateGraphicColor;
				else if (GetComponent<Text> () != null)
					displayColor = UpdateTextColor;
				else if (GetComponent<SpriteRenderer> () != null)
					displayColor = UpdateSpriteRendererColor;
				else if (GetComponent<TextMesh> () != null)
					displayColor = UpdateTextMeshColor;
				else if (GetComponent<Renderer> () != null && GetComponent<Renderer> ().sharedMaterial.HasProperty (materialPropertyName))
					displayColor = UpdateRendererColor;
				else if (GetComponent<MeshFilter> () != null)
					displayColor = UpdateMeshVertexColor;
				else if (GetComponent<Light> () != null)
					displayColor = UpdateLightColor;
				else if (GetComponent<ParticleSystem> () != null)
					displayColor = UpdateParticleSystemColor;
				else if (GetComponent<LensFlare> () != null)
					displayColor = UpdateLensFlareColor;
				else if (GetComponent<GUIText> () != null)
					displayColor = UpdateGuiTextColor;
				else if (GetComponent<GUITexture> () != null)
					displayColor = UpdateGuiTextColor;
				else
					this.Log ("Cannot find color update target.");
			}
			return displayColor;
		}
	}

	private void UpdateRendererColor (Color color)
	{
		Material material = tintSharedMaterial ? GetComponent<Renderer> ().sharedMaterial : GetComponent<Renderer> ().material;
		material.SetColor (materialPropertyName, color);
	}

	private void UpdateGraphicColor (Color color)
	{
		GetComponent<Graphic> ().color = color;
	}

	private void UpdateSpriteRendererColor (Color color)
	{
		GetComponent<SpriteRenderer> ().color = color;
	}

	private void UpdateTextColor (Color color)
	{
		GetComponent<Text> ().color = color;
	}

	private void UpdateTextMeshColor (Color color)
	{
		GetComponent<TextMesh> ().color = color;
	}

	private void UpdateMeshVertexColor (Color color)
	{
		MeshUtility.Tint (GetComponent<MeshFilter> ().mesh, color);
	}

	private void UpdateLightColor (Color color)
	{
		GetComponent<Light> ().color = color;
	}

	private void UpdateParticleSystemColor (Color color)
	{
		GetComponent<ParticleSystem> ().startColor = color;
	}

	private void UpdateLensFlareColor (Color color)
	{
		GetComponent<LensFlare> ().color = color;
	}

	private void UpdateGuiTextureColor (Color color)
	{
		GetComponent<GUITexture> ().color = color;
	}

	private void UpdateGuiTextColor (Color color)
	{
		GetComponent<GUIText> ().color = color;
	}

	public void Display (Color color)
	{
		DisplayColor (color);
	}

	[ContextMenu ("Show Display Color Method")]
	private void LogDisplayColorDelegate ()
	{
		this.Log (DisplayColor.Method.Name);
	}
}
