using UnityEngine;
using System.Collections;

public class ScaleTimeCommand : CoroutineCommandBehavior
{
	public float timeScale = 0;
	public float duration = 1;

	protected override IEnumerator CoroutineCommand ()
	{
		float timeScaleBefore = Time.timeScale;
		Time.timeScale = timeScale;

		float timeStart = Time.unscaledTime;
		while (Time.unscaledTime - timeStart < duration)
			yield return null;

		Time.timeScale = timeScaleBefore;
	}
}
