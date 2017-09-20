using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class XboxEngagerEventTrigger : MonoBehaviour
{
	public XboxEngager engager;
	public UnityEvent onControllerStart;
	public UnityEvent onControllerComplete;

	void Start ()
	{
		engager.ControllerEngageStarted += HandleControllerEngageStarted;
		engager.ControllerEngageCompleted += HandleControllerEngageCompleted;
	}

	void OnDestroy ()
	{
		engager.ControllerEngageStarted -= HandleControllerEngageStarted;
		engager.ControllerEngageCompleted -= HandleControllerEngageCompleted;
	}

	void HandleControllerEngageStarted (XboxEngager engager)
	{
		onControllerStart.Invoke ();
	}

	void HandleControllerEngageCompleted (XboxEngager engager)
	{
		onControllerComplete.Invoke ();
	}
}
