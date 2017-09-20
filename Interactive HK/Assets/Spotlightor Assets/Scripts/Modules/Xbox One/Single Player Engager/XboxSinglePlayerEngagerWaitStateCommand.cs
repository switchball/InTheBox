using UnityEngine;
using System.Collections;

public class XboxSinglePlayerEngagerWaitStateCommand : CoroutineCommandBehavior
{
	public XboxSinglePlayerEngager.StateTypes targetStateType;

	protected override IEnumerator CoroutineCommand ()
	{
		if (XboxSinglePlayerEngager.Instance != null) {
			while (XboxSinglePlayerEngager.Instance.FiniteStateMachine.CurrentState.StateId != targetStateType)
				yield return null;
		}
	}
}
