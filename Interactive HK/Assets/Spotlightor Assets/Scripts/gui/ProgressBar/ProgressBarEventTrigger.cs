using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ProgressBarEventTrigger : MonoBehaviour
{
	public ProgressBar progressBar;
	public UnityEvent tweenStarted;
	public UnityEvent tweenCompleted;

	void Awake ()
	{
		progressBar.TweenStarted += HandleTweenStarted;
		progressBar.TweenCompleted += HandleTweenCompleted;
	}

	void HandleTweenStarted (ProgressBar progressBar, float progressFrom, float progressTo)
	{
		tweenStarted.Invoke ();
	}
	
	void HandleTweenCompleted (ProgressBar progressBar)
	{
		tweenCompleted.Invoke ();
	}

}
