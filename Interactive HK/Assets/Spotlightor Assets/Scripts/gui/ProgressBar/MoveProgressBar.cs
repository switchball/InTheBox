using UnityEngine;
using System.Collections;

public class MoveProgressBar : ProgressBar
{
	public Vector3 posEmpty;
	public Vector3 posFull;

	protected override void UpdateProgressDisplay (float progress)
	{
		transform.localPosition = Vector3.Lerp (posEmpty, posFull, progress);
	}

	void Reset ()
	{
		posEmpty = posFull = transform.localPosition;
	}

	[ContextMenu ("Set Pos Empty")]
	void SetPositionEmpty ()
	{
		posEmpty = transform.localPosition;
	}

	[ContextMenu ("Set Pos Full")]
	void SetPositionFull ()
	{
		posFull = transform.localPosition;
	}
}
