using UnityEngine;
using System.Collections;

public class XboxSinglePlayerEngagerStateWatch : XboxSinglePlayerEngagerState
{
	public override XboxSinglePlayerEngager.StateTypes StateId {
		get { return XboxSinglePlayerEngager.StateTypes.Watch; }
	}

	#if UNITY_XBOXONE
	public override void BeginState (XboxSinglePlayerEngager.StateTypes previousState)
	{
		base.BeginState (previousState);

		if (Owner.ActivePlayer.UserId > -1) {
			if (Owner.ActivePlayer.IsControllerUserPairValid) {
				Owner.ActivePlayer.BecameInvalid += HandleActivePlayerBecameInvalid;
				XboxOnePLM.OnWindowActivatedChangedEvent += HandleWindowActivatedChangedEvent;
			} else {
				this.Log ("ActivePlayer not valid: {0}. Goto ReEngage state.", Owner.ActivePlayer);
				Owner.GotoState (XboxSinglePlayerEngager.StateTypes.ReEngage);
			}
		} else {
			this.Log ("ActivePlayer.UserId is not valid: {0}. Nothing to watch, Goto NotSet state.", Owner.ActivePlayer);
			Owner.GotoState (XboxSinglePlayerEngager.StateTypes.NotSet);
		}
	}

	public override void EndState (XboxSinglePlayerEngager.StateTypes newState)
	{
		Owner.ActivePlayer.BecameInvalid -= HandleActivePlayerBecameInvalid;
		XboxOnePLM.OnWindowActivatedChangedEvent -= HandleWindowActivatedChangedEvent;

		base.EndState (newState);
	}

	void HandleWindowActivatedChangedEvent (XboxOneCoreWindowActivationState windowActivationState)
	{
		if (windowActivationState == XboxOneCoreWindowActivationState.Deactivated) {
			this.Log ("Window activation state = {0}. Goto ReEngage state.", windowActivationState);
			Owner.GotoState (XboxSinglePlayerEngager.StateTypes.ReEngage);
		}
	}

	void HandleActivePlayerBecameInvalid (XboxPlayer xboxPlayer)
	{
		if (Owner.ActivePlayer.UserId > -1) {
			this.Log ("ActivePlayer became invalid: {0}. Goto ReEngage state.", xboxPlayer);
			Owner.GotoState (XboxSinglePlayerEngager.StateTypes.ReEngage);
		} else {
			this.Log ("ActivePlayer became invalid and UserId is invalid: {0}. Goto NotSet state.", xboxPlayer);
			Owner.GotoState (XboxSinglePlayerEngager.StateTypes.NotSet);
		}
	}
	#endif
}
