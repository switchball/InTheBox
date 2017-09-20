using UnityEngine;
using System.Collections;

public class WaitCommandCommand : CoroutineCommandBehavior
{
	public CoroutineCommandBehavior command;
	public bool waitForExecute = true;

	protected override IEnumerator CoroutineCommand ()
	{
		if (waitForExecute) {
			while (!command.IsExecuting)
				yield return null;
		}else{
			while (command.IsExecuting)
				yield return null;
		}
	}
}
