using UnityEngine;
using System.Collections;

public abstract class ShakeDecorator<T> : Shake where T: Shake
{
	public T shake;
	
	void Awake ()
	{
		shake.Started += HandleStarted;
		shake.Ended += HandleEnded;

		if (shake.IsShaking)
			HandleStarted (shake);
	}
	
	void HandleStarted (Shake shake)
	{
		OnStarted ();
	}

	void HandleEnded (Shake shake)
	{
		OnEnded ();
	}

	void Reset ()
	{
		if (shake == null) {
			T[] childShakes = GetComponentsInChildren<T> (true);
			foreach (T childShake in childShakes) {
				if (childShake != this) {
					shake = childShake;
					break;
				}
			}
		}
	}
}
