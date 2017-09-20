using UnityEngine;
using System.Collections;

public class XboxSinglePlayerEngagerStateSetup : XboxSinglePlayerEngagerState
{
	public override XboxSinglePlayerEngager.StateTypes StateId {
		get { return XboxSinglePlayerEngager.StateTypes.Setup; }
	}

	private bool completed = false;

	public override void BeginState (XboxSinglePlayerEngager.StateTypes previousState)
	{
		base.BeginState (previousState);

		StartCoroutine ("SetupActivePlayer");
	}

	public override void EndState (XboxSinglePlayerEngager.StateTypes newState)
	{
		completed = false;
		Owner.engager.Completed -= HandleEngagerCompleted;
		StopCoroutine ("SetupActivePlayer");

		base.EndState (newState);
	}

	private IEnumerator SetupActivePlayer ()
	{
		this.Log ("Setup Start");

		if (Owner.engager.IsEngaging) {
			this.Log ("Engager is engaging, let's wait...");

			while (Owner.engager.IsEngaging)
				yield return null;
			
			this.Log ("Engager finished engaging, let's continue");
		}

		Owner.engager.Completed += HandleEngagerCompleted;

		do {
			this.Log ("Start engage for compelte new user-controlelr pair");

			completed = false;
			if (Owner.engager.EngagedUserId <= 0) {
				Owner.engager.Engage ();
			} else {
				this.Log ("Setup enage for target user id: {0}", Owner.engager.EngagedUserId);
				Owner.engager.EngageForUser (Owner.engager.EngagedUserId);
			}
			
			while (!completed)
				yield return null;
		} while(!Owner.engager.IsEngagedControllerUserPairValid);

		Owner.engager.Completed -= HandleEngagerCompleted;

		Owner.ActivePlayer.ControllerId = Owner.engager.EngagedControllerId;
		Owner.ActivePlayer.UserId = Owner.engager.EngagedUserId;

		this.Log ("Setup finished. ActivePlayer: {0}", Owner.ActivePlayer);

		Owner.GotoState (XboxSinglePlayerEngager.StateTypes.Set);
	}

	void HandleEngagerCompleted (XboxEngager engager)
	{
		completed = true;
		this.Log ("Engage completed. ValidPair = {0}, UserId = {1}, ControllerId = {2}", 
			engager.IsEngagedControllerUserPairValid, engager.EngagedUserId, engager.EngagedControllerId);
	}
}
