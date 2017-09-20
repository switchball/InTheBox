using UnityEngine;
using System.Collections;

public class WaitTimeCommand : CoroutineCommandBehavior
{
	public float time = 1;

	protected override IEnumerator CoroutineCommand ()
	{
		yield return new WaitForSeconds (time);
	}
}
