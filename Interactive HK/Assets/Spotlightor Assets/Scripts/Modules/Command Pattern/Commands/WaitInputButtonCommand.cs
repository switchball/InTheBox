using UnityEngine;
using System.Collections;

public class WaitInputButtonCommand : CoroutineCommandBehavior
{
	public string buttonName = "Submit";

	protected override IEnumerator CoroutineCommand ()
	{
		while (Input.GetButtonDown (buttonName) == false)
			yield return null;
	}
}
