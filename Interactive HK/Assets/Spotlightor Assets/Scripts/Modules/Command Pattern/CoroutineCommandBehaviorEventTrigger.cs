using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CoroutineCommandBehaviorEventTrigger : MonoBehaviour
{
	public CoroutineCommandBehavior target;
	public UnityEvent started;
	public UnityEvent ended;

	void Awake ()
	{
		if (target == null)
			target = GetComponent<CoroutineCommandBehavior> ();

		if (target != null) {
			target.Started += HandleStarted;
			target.Ended += HandleEnded;
		}
	}

	void HandleStarted (CoroutineCommandBehavior source)
	{
		started.Invoke ();
	}
	
	void HandleEnded (CoroutineCommandBehavior source)
	{
		ended.Invoke ();
	}

	void Reset ()
	{
		if (target == null)
			target = GetComponent<CoroutineCommandBehavior> ();
	}
}
