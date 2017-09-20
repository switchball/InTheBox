using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ContentGosActivatorOnCondition<T> : ContentDisplayerChildBehavior<T>
{
	public List<GameObject> activeObjectsIfTrue;
	public List<GameObject> activeObjectsIfFalse;

	public override void UpdateDisplay (T content)
	{
		bool hasMetCondition = ContentHasMetCondition (content);

		activeObjectsIfTrue.ForEach (obj => obj.SetActive (hasMetCondition));
		activeObjectsIfFalse.ForEach (obj => obj.SetActive (!hasMetCondition));
	}

	protected abstract bool ContentHasMetCondition (T content);
}
