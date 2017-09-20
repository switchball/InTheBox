using UnityEngine;
using System.Collections;

public class TextureOffsetProgressBar : ProgressBar
{
	public float emptyOffsetX = -0.003f;
	public float fullOffsetX = 1;

	protected override void UpdateProgressDisplay (float progress)
	{
		Vector2 offset = GetComponent<Renderer>().material.mainTextureOffset;
		offset.x = Mathf.Lerp (emptyOffsetX, fullOffsetX, progress);
		GetComponent<Renderer>().material.mainTextureOffset = offset;
	}
}
