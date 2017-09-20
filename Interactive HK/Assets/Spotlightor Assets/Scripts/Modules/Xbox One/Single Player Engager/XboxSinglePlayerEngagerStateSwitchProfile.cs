using UnityEngine;
using System.Collections;

public class XboxSinglePlayerEngagerStateSwitchProfile : XboxSinglePlayerEngagerState
{
	public override XboxSinglePlayerEngager.StateTypes StateId {
		get { return XboxSinglePlayerEngager.StateTypes.SwitchProfile; }
	}

	private bool completed = false;

	public override void BeginState (XboxSinglePlayerEngager.StateTypes previousState)
	{
		base.BeginState (previousState);

		if (Owner.ActivePlayer.IsControllerUserPairValid) {
			StartCoroutine ("SwitchProfile");
		} else {
			this.Log ("ActivePlayer not valid: {0}. Goto NotSet", Owner.ActivePlayer);
			Owner.GotoState (XboxSinglePlayerEngager.StateTypes.NotSet);
		}
	}

	public override void EndState (XboxSinglePlayerEngager.StateTypes newState)
	{
		completed = false;
		Owner.engager.Completed -= HandleEngagerCompleted;
		StopCoroutine ("SwitchProfile");

		base.EndState (newState);
	}

	private IEnumerator SwitchProfile ()
	{
		completed = false;

		if (Owner.engager.IsEngaging) {
			this.Log ("Engager is engaging, let's wait...");

			while (Owner.engager.IsEngaging)
				yield return null;

			this.Log ("Engager finished engaging, let's continue");
		}

		Owner.engager.Completed += HandleEngagerCompleted;
		Owner.engager.SwitchProfile (Owner.ActivePlayer.ControllerId);

		while (!completed)
			yield return null;

		Owner.engager.Completed -= HandleEngagerCompleted;

		int engagedUserId = Owner.engager.EngagedUserId;
		if (Owner.engager.IsEngagedControllerUserPairValid) {
			if (engagedUserId != Owner.ActivePlayer.UserId) {
				this.Log ("Switched to new User: {0} from {1}. Goto NotSet State.", engagedUserId, Owner.ActivePlayer.UserId);
				Owner.GotoState (XboxSinglePlayerEngager.StateTypes.NotSet);
			} else {
				this.Log ("User didn't change and engagement is still valid. Update controllerID and goto Set state.");
				Owner.ActivePlayer.ControllerId = Owner.engager.EngagedControllerId;
				Owner.GotoState (XboxSinglePlayerEngager.StateTypes.Set);
			}
		} else {
			if (Owner.ActivePlayer.IsControllerUserPairValid) {
				this.Log ("New engagement is not a valid pair, but ActivePlayer is valid. Goto Set state.");
				Owner.GotoState (XboxSinglePlayerEngager.StateTypes.Set);
			} else {
				this.Log ("New engagement and ActivePlayer is not valid. Goto NotSet state.");
				Owner.GotoState (XboxSinglePlayerEngager.StateTypes.NotSet);
			}
		}
	}

	void HandleEngagerCompleted (XboxEngager engager)
	{
		completed = true;
		this.Log ("SwitchProfile completed. ValidPair = {0}, UserId = {1}, ControllerId = {2}", 
			engager.IsEngagedControllerUserPairValid, engager.EngagedUserId, engager.EngagedControllerId);
	}
}
