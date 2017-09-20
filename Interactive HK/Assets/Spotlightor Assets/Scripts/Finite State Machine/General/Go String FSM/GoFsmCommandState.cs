using UnityEngine;
using System.Collections;

public class GoFsmCommandState : GoFsmState
{
	public CoroutineCommandBehavior stateCommand;
	public GoFsmState autoNextState = null;

	public override void BeginState (int previousState)
	{
		base.BeginState (previousState);
		StartCoroutine ("ExecuteStateCommand");
	}

	public override void EndState (int newState)
	{
		StopCoroutine ("ExecuteStateCommand");
		base.EndState (newState);
	}

	private IEnumerator ExecuteStateCommand ()
	{
		if (stateCommand != null)
			yield return StartCoroutine (stateCommand.ExecuteCoroutine ());

		if (autoNextState != null)
			Owner.GotoState (autoNextState);
	}
}
