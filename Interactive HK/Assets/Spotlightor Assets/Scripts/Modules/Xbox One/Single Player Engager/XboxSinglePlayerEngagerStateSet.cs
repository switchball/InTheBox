using UnityEngine;
using System.Collections;

public class XboxSinglePlayerEngagerStateSet : XboxSinglePlayerEngagerState
{
	public override XboxSinglePlayerEngager.StateTypes StateId {
		get { return XboxSinglePlayerEngager.StateTypes.Set; }
	}

	public override void BeginState (XboxSinglePlayerEngager.StateTypes previousState)
	{
		base.BeginState (previousState);

		if (Owner.ActivePlayer.IsControllerUserPairValid) {
			Owner.ActivePlayer.BecameInvalid += HandleActivePlayerBecameInvalid;
		} else {
			this.Log ("ActivePlayer not valid: {0}. Go back to NotSet state.", Owner.ActivePlayer);
			Owner.GotoState (XboxSinglePlayerEngager.StateTypes.NotSet);
		}
	}

	public override void EndState (XboxSinglePlayerEngager.StateTypes newState)
	{
		Owner.ActivePlayer.BecameInvalid -= HandleActivePlayerBecameInvalid;

		base.EndState (newState);
	}

	void HandleActivePlayerBecameInvalid (XboxPlayer xboxPlayer)
	{
		this.Log ("ActivePlayer became invalid: {0}. Goto NotSet state.", xboxPlayer);

		Owner.GotoState (XboxSinglePlayerEngager.StateTypes.NotSet);
	}
}
