using UnityEngine;
using System.Collections;

public abstract class CommandBehavior : MonoBehaviour, ICommand
{
	public bool executeOnAwake = false;

	protected virtual void Start ()
	{
		if (executeOnAwake)
			Execute ();
	}

	public abstract void Execute ();
}
