using UnityEngine;
using System.Collections;

public class XboxSinglePlayerEngagerGotoStateCommand : CoroutineCommandBehavior
{
	public XboxSinglePlayerEngager.StateTypes targetStateType;

	protected override IEnumerator CoroutineCommand ()
	{
		if (XboxSinglePlayerEngager.Instance != null)
			XboxSinglePlayerEngager.Instance.GotoState (targetStateType);
		
		yield return null;
	}
}
