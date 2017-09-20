using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class XboxSinglePlayerEngagerStateReEngage : XboxSinglePlayerEngagerState
{
	public override XboxSinglePlayerEngager.StateTypes StateId {
		get { return XboxSinglePlayerEngager.StateTypes.ReEngage; }
	}

	private bool completed = false;


	public override void BeginState (XboxSinglePlayerEngager.StateTypes previousState)
	{
		base.BeginState (previousState);

		if (Owner.ActivePlayer.IsControllerUserPairValid == false) {
			if (Owner.ActivePlayer.UserId > -1)
				StartCoroutine ("ReEngage");
			else {
				this.Log ("ActivePlayer.UserId is not set: {0}. Cannot re-engage. Goto NotSet state", Owner.ActivePlayer);
				Owner.GotoState (XboxSinglePlayerEngager.StateTypes.NotSet);
			}
		} else {
			StartCoroutine ("ReEngage");
		}
	}

	public override void EndState (XboxSinglePlayerEngager.StateTypes newState)
	{
		completed = false;
		Owner.engager.Completed -= HandleEngagerCompleted;
		StopCoroutine ("ReEngage");

		base.EndState (newState);
	}

	private IEnumerator ReEngage ()
	{
		completed = false;

		if (Owner.engager.IsEngaging) {
			this.Log ("Engager is engaging, let's wait...");

			while (Owner.engager.IsEngaging)
				yield return null;

			this.Log ("Engager finished engaging, let's continue");
		}

		Owner.engager.Completed += HandleEngagerCompleted;
		do {
			this.Log ("Start re-engage for XboxPlayer: {0}", Owner.ActivePlayer);

			completed = false;
			Owner.engager.EngageForControllerAndPlayer (Owner.ActivePlayer.ControllerId, Owner.ActivePlayer.UserId);

			while (!completed)
				yield return null;
		} while(!Owner.engager.IsEngagedControllerUserPairValid);
		Owner.engager.Completed -= HandleEngagerCompleted;

		Owner.ActivePlayer.ControllerId = Owner.engager.EngagedControllerId;
		int engagedUserId = Owner.engager.EngagedUserId;
		if (engagedUserId == Owner.ActivePlayer.UserId) {
			this.Log ("Same user {0} re-engaged. Update controllerID. Goto Watch state.", engagedUserId);
			Owner.GotoState (XboxSinglePlayerEngager.StateTypes.Watch);
		} else {
			this.Log ("Switched to new User: {0} from {1}. Goto NotSet State.", engagedUserId, Owner.ActivePlayer.UserId);
			Owner.GotoState (XboxSinglePlayerEngager.StateTypes.NotSet);
		}
	}

	void HandleEngagerCompleted (XboxEngager engager)
	{
		completed = true;
		this.Log ("Engage completed. ValidPair = {0}, UserId = {1}, ControllerId = {2}", 
			engager.IsEngagedControllerUserPairValid, engager.EngagedUserId, engager.EngagedControllerId);
	}
}
