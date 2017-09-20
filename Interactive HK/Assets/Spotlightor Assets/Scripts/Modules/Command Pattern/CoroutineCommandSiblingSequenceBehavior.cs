using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoroutineCommandSiblingSequenceBehavior : CoroutineCommandBehavior
{
	public bool loop = false;

	protected override IEnumerator CoroutineCommand ()
	{
		do {
			List<CoroutineCommandBehavior> commandSequence = new List<CoroutineCommandBehavior> ();
			for (int i = 0; i < transform.childCount; i++) {
				CoroutineCommandBehavior childCommand = transform.GetChild (i).GetComponent<CoroutineCommandBehavior> ();
				if (childCommand != null)
					commandSequence.Add (childCommand);
			}

			foreach (CoroutineCommandBehavior command in commandSequence) {
				if (command.gameObject.activeSelf)
					yield return StartCoroutine (command.ExecuteCoroutine ());
			}
		} while(loop);
	}
}
