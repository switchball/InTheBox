using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextDisplayer))]
public abstract class ContentTextPropertyDisplayer<T> : ContentDisplayerChildBehavior<T>
{
	public override void UpdateDisplay (T content)
	{
		GetComponent<TextDisplayer> ().Text = GetTextToDisplay (content);
	}

	protected abstract string GetTextToDisplay (T content);
}
