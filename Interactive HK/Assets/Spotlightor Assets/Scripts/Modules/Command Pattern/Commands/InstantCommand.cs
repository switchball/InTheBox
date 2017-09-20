using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InstantCommand : CoroutineCommandBehavior
{
	public UnityEvent started;
	public int delayFrames = 0;

	protected override IEnumerator CoroutineCommand ()
	{
		started.Invoke ();

		int framesDelayed = 0;
		while (framesDelayed < delayFrames) {
			yield return null;
			framesDelayed++;
		}
	}
}
