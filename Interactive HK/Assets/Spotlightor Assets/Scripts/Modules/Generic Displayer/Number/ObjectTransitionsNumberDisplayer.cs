using UnityEngine;
using System.Collections;

public class ObjectTransitionsNumberDisplayer : NumberDisplayer
{
	public TransitionManager[] transitions;

	protected override void Display (float value)
	{
		int intValue = Mathf.RoundToInt (value);

		for (int i = 0; i < transitions.Length; i++) {
			if (i < intValue)
				transitions [i].TransitionIn ();
			else
				transitions [i].TransitionOut ();
		}

		if (intValue < 0 || intValue > transitions.Length) 
			this.LogWarning ("Display value {0} out of display range: {1}~{2}", value, 0, transitions.Length);
	}

	void Reset ()
	{
		transitions = GetComponentsInChildren<TransitionManager> ();
	}
}
