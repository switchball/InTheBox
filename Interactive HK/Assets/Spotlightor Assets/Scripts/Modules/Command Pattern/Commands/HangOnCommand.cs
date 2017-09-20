using UnityEngine;
using System.Collections;

public class HangOnCommand : CoroutineCommandBehavior
{
	private bool isHangOn = false;

	protected override IEnumerator CoroutineCommand ()
	{
		isHangOn = true;

		while (isHangOn)
			yield return null;
	}

	public void StopHangOn ()
	{
		isHangOn = false;
	}
}
