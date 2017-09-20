using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TransitionManager))]
public class TransitionableMonoBehavior : MonoBehaviour, ITransition
{
	public event TransitionManager.TransitionBegunEventHandler TransitionInTriggered {
		add{ Transition.TransitionInTriggered += value;}
		remove{ Transition.TransitionInTriggered -= value;}
	}

	public event TransitionManager.TransitionBegunEventHandler TransitionInStarted {
		add{ Transition.TransitionInStarted += value;}
		remove{ Transition.TransitionInStarted -= value;}
	}

	public event TransitionManager.GenericEventHandler TransitionInCompleted {
		add{ Transition.TransitionInCompleted += value;}
		remove{ Transition.TransitionInCompleted -= value;}
	}

	public event TransitionManager.TransitionBegunEventHandler TransitionOutTriggered {
		add{ Transition.TransitionOutTriggered += value;}
		remove{ Transition.TransitionOutTriggered -= value;}
	}

	public event TransitionManager.TransitionBegunEventHandler TransitionOutStarted {
		add{ Transition.TransitionOutStarted += value;}
		remove{ Transition.TransitionOutStarted -= value;}
	}

	public event TransitionManager.GenericEventHandler TransitionOutCompleted {
		add{ Transition.TransitionOutCompleted += value;}
		remove{ Transition.TransitionOutCompleted -= value;}
	}

	private TransitionManager transition;

	public TransitionManager Transition {
		get {
			if (transition == null) {
				transition = GetComponent<TransitionManager> ();
				if (transition == null)
					Debug.LogError ("No TransitionManager in " + name);
			}
			return transition; 
		}
	}

	public void TransitionIn ()
	{
		Transition.TransitionIn ();
	}

	public void TransitionIn (bool instant)
	{
		Transition.TransitionIn (instant);
	}

	public void TransitionOut ()
	{
		Transition.TransitionOut ();
	}

	public void TransitionOut (bool instant)
	{
		Transition.TransitionOut (instant);
	}
}
