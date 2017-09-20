using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoroutineCommandSequenceBehavior : CoroutineCommandBehavior
{
	public List<CoroutineCommandBehavior> commandSequence;

	protected override IEnumerator CoroutineCommand ()
	{
		foreach (CoroutineCommandBehavior command in commandSequence)
			yield return StartCoroutine (command.ExecuteCoroutine ());
	}

	void Reset ()
	{
		commandSequence = new List<CoroutineCommandBehavior> (GetComponentsInChildren<CoroutineCommandBehavior> (true));
		commandSequence.Remove (this);
	}
}
