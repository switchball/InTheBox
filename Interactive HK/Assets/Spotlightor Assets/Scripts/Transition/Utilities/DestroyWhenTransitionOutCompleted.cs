using UnityEngine;
using System.Collections;

public class DestroyWhenTransitionOutCompleted : MonoBehaviour
{
	void Start ()
	{
		GetComponent<TransitionManager> ().TransitionOutCompleted += HandleTransitionOutCompleted;
	}

	void HandleTransitionOutCompleted (TransitionManager source)
	{
		Destroy (gameObject);
	}
}
