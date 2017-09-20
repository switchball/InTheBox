using UnityEngine;
using System.Collections;

public abstract class Shake : MonoBehaviour
{
	public delegate void BasicEventHandler (Shake shake);
	
	public event BasicEventHandler Started;
	public event BasicEventHandler Ended;
	
	public Vector3 Intensity {
		get{ return IsShaking ? GetCurrentIntensity () : Vector3.zero;}
	}

	public bool IsShaking{ get; private set; }
	
	protected void OnStarted ()
	{
		HookBeforeStarted();

		IsShaking = true;

		if (Started != null)
			Started (this);
	}

	protected virtual void HookBeforeStarted ()
	{
	}

	protected void OnEnded ()
	{
		IsShaking = false;

		if (Ended != null)
			Ended (this);
	}

	protected abstract Vector3 GetCurrentIntensity ();
}
