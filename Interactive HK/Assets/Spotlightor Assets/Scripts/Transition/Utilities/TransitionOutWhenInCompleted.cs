using UnityEngine;
using System.Collections;

[RequireComponent (typeof(TransitionManager))]
public class TransitionOutWhenInCompleted : MonoBehaviour
{
	public float delay = 0;
	public bool ignoreTimeScale = false;
	private IEnumerator timeDelayRoutine = null;

	void Awake ()
	{
		GetComponent<TransitionManager> ().TransitionInCompleted += HandleTransitionInCompleted;
		GetComponent<TransitionManager> ().TransitionOutTriggered += HandleTransitionOutTriggered;
	}

	void HandleTransitionOutTriggered (TransitionManager source, bool isInstant, TransitionManager.StateTypes prevStateType)
	{
		if (timeDelayRoutine != null)
			StopCoroutine (timeDelayRoutine);
	}

	void HandleTransitionInCompleted (TransitionManager source)
	{
		if (delay <= 0)
			GetComponent<TransitionManager> ().TransitionOut ();
		else {
			if (timeDelayRoutine != null)
				StopCoroutine (timeDelayRoutine);

			timeDelayRoutine = DelayAndTransitionOut ();
			StartCoroutine (timeDelayRoutine);
		}
	}

	private IEnumerator DelayAndTransitionOut ()
	{
		float timeElapsed = 0;
		while (timeElapsed < delay) {
			yield return null;
			float deltaTime = ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
			timeElapsed += deltaTime;
		}

		GetComponent<TransitionManager> ().TransitionOut ();
	}
}
