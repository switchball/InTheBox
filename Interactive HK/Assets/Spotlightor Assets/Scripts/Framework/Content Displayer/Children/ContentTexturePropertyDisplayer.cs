using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextureDisplayer))]
public abstract class ContentTexturePropertyDisplayer<T> : ContentDisplayerChildBehavior<T>
{
	public override void UpdateDisplay (T content)
	{
		GetComponent<TextureDisplayer> ().Display (GetTextureToDisplay (content));
	}

	protected abstract Texture GetTextureToDisplay (T content);
}
