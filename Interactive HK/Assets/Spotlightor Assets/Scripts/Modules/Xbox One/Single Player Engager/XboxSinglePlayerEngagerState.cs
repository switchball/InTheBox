using UnityEngine;
using System.Collections;

public abstract class XboxSinglePlayerEngagerState : SimpleFsmStateMonobehavior<XboxSinglePlayerEngager, XboxSinglePlayerEngager.StateTypes>
{
	public override void BeginState (XboxSinglePlayerEngager.StateTypes previousState)
	{
		this.Log ("Goto state [{0}] from [{1}]", this.StateId, previousState);
		base.BeginState (previousState);
	}
}
