using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TransitionManagerEventTrigger : MonoBehaviour
{
	public TransitionManager transitionManager;
	public UnityEvent transitionInStarted;
	public UnityEvent transitionInCompleted;
	public UnityEvent transitionOutStarted;
	public UnityEvent transitionOutCompleted;

	private bool ShouldInvokeEvents {
		get {
			bool should = false;
			if (this.enabled && gameObject.activeInHierarchy)
				should = true;
			else
				should = transitionManager.gameObject == this.gameObject;
			return should;
		}
	}

	void Awake ()
	{
		if (transitionManager == null)
			transitionManager = GetComponent<TransitionManager> ();

		if (transitionManager != null) {
			transitionManager.TransitionInStarted += HandleTransitionInStarted;
			transitionManager.TransitionInCompleted += HandleTransitionInCompleted;
			transitionManager.TransitionOutStarted += HandleTransitionOutStarted;
			transitionManager.TransitionOutCompleted += HandleTransitionOutCompleted;
		}
	}

	void HandleTransitionInStarted (TransitionManager source, bool isInstant, TransitionManager.StateTypes prevStateType)
	{
		if (ShouldInvokeEvents)
			transitionInStarted.Invoke ();
	}

	void HandleTransitionInCompleted (TransitionManager source)
	{
		if (ShouldInvokeEvents)
			transitionInCompleted.Invoke ();
	}

	void HandleTransitionOutStarted (TransitionManager source, bool isInstant, TransitionManager.StateTypes prevStateType)
	{
		if (ShouldInvokeEvents)
			transitionOutStarted.Invoke ();
	}

	void HandleTransitionOutCompleted (TransitionManager source)
	{
		if (ShouldInvokeEvents)
			transitionOutCompleted.Invoke ();
	}

	void Reset ()
	{
		if (transitionManager == null) {
			transitionManager = GetComponent<TransitionManager> ();
			if (transitionManager == null)
				transitionManager = GetComponentInChildren<TransitionManager> ();
		}
	}
}
