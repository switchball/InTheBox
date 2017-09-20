using UnityEngine;
using System.Collections;

public class XboxSinglePlayerEngagerStateNotSet : XboxSinglePlayerEngagerState
{
	public override XboxSinglePlayerEngager.StateTypes StateId {
		get { return XboxSinglePlayerEngager.StateTypes.NotSet; }
	}

	public override void BeginState (XboxSinglePlayerEngager.StateTypes previousState)
	{
		base.BeginState (previousState);

		if (Owner.ActivePlayer.HasSet)
			Owner.ActivePlayer.Clear ();
	}
}
