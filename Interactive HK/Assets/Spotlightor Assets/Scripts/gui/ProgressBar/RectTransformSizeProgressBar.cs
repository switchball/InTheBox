using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RectTransformSizeProgressBar : ProgressBar
{
	public RectTransform bar;
	public Vector2 emptySize;
	public Vector2 fullSize;

	protected override void UpdateProgressDisplay (float progress)
	{
		bar.sizeDelta = Vector2.Lerp (emptySize, fullSize, progress);
	}

#if UNITY_EDITOR
	[ContextMenu("Update Empty Size")]
	private void UpdateEmptySize ()
	{
		emptySize = bar.sizeDelta;
	}

	[ContextMenu("Update Full Size")]
	private void UpdateFullSize ()
	{
		fullSize = bar.sizeDelta;
	}
#endif
}
