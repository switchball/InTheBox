using UnityEngine;
using System.Collections;

/// <summary>
/// Want to use the active/enable to trigger some behaviors, but the OnEnable/OnDisable is quit UNRELIABLE?
/// Use this instead of the MonoBehaviour and override OnBecameFunctional/OnBecameUnFunctional to implement the behaviors.
/// OnBecameFunctional will only be triggered by Start or OnEnable(when has started before)
/// OnBecameUnFunctional will only be triggered by OnDisable(when has started and application not quit yet).
/// </summary>
public abstract class FunctionalMonoBehaviour : MonoBehaviour
{
	private bool started = false;
	private bool applicationHasQuit = false;
	// Use this for initialization
	protected virtual void Start ()
	{
		started = true;
		OnBecameFunctional (true);
	}
	
	protected virtual void OnApplicationQuit ()
	{
		applicationHasQuit = true;
	}

	protected virtual void OnEnable ()
	{
		if (!started)
			return;
		OnBecameFunctional (false);
	}

	protected virtual void OnDisable ()
	{
		if (!started || applicationHasQuit 
			#if UNITY_5
			#else
			|| Application.isLoadingLevel
			#endif
			//TODO Need a method to detect isLoadingLevel
		)
			return;
		OnBecameUnFunctional ();
	}

	protected abstract void OnBecameFunctional (bool forTheFirstTime);

	protected abstract void OnBecameUnFunctional ();
}
