using UnityEngine;
using System.Collections;

public class TransitionManagerTester : EasyDebugGui
{
	public TransitionManager transition;

	protected override void DrawDebugGUI ()
	{
		if (transition != null) {
			Label (string.Format ("{0} [{1}]", transition.name, transition.State), 200);

			if (Button ("In", 50))
				transition.TransitionIn ();
			if (Button ("Out", 50))
				transition.TransitionOut ();
		}
	}

	protected override void Update ()
	{
		base.Update ();

		if (transition != null) {
			if (Input.GetKeyDown (KeyCode.I))
				transition.TransitionIn ();
			if (Input.GetKeyDown (KeyCode.O))
				transition.TransitionOut ();
		}
	}

	void Reset ()
	{
		if (transition == null)
			transition = GetComponent<TransitionManager> ();
	}
}
