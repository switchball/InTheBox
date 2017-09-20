using UnityEngine;
using System.Collections;

public class GlobalBlackScreenTransitionFunctions : MonoBehaviour
{
	public void TransitionIn ()
	{
		TransitionIn (false);
	}

	public void TransitionIn (bool instant)
	{
		GlobalBlackScreenTransition.Instance.TransitionIn (instant);
	}

	public void TransitionOut ()
	{
		TransitionOut (false);
	}

	public void TransitionOut (bool instant)
	{
		GlobalBlackScreenTransition.Instance.TransitionOut (instant);
	}
}
