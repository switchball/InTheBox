using UnityEngine;
using System.Collections;

public abstract class CoroutineCommandBehavior : CommandBehavior
{
	public delegate void BasicEventHandler (CoroutineCommandBehavior source);

	public event BasicEventHandler Started;
	public event BasicEventHandler Ended;

	private bool isExecuting = false;

	public bool IsExecuting { 
		get{ return this.isExecuting; } 
		private set {
			if (isExecuting != value) {
				this.isExecuting = value;
				#if UNITY_EDITOR
				if (this.isExecuting)
					this.name = this.name + " <";
				else
					this.name = this.name.Replace (" <", "");
				#endif
			}
		}
	}

	public override void Execute ()
	{
		if (IsExecuting == false)
			StartCoroutine ("ExecuteCoroutine");
		else
			this.LogWarning ("Command {0} is executing, cannot Execute again!", this);
	}

	public IEnumerator ExecuteCoroutine ()
	{
		if (IsExecuting == false) {
			IsExecuting = true;
			if (Started != null)
				Started (this);

			yield return StartCoroutine ("CoroutineCommand");

			IsExecuting = false;
			if (Ended != null)
				Ended (this);
		} else
			this.LogWarning ("Command {0} is executing, cannot Execute again!", this);
	}

	private void OnDisable ()
	{
		if (IsExecuting)
			Stop ();
	}

	private void Stop ()
	{
		StopCoroutine ("ExecuteCoroutine");
		StopCoroutine ("CoroutineCommand");
		IsExecuting = false;
	}

	protected abstract IEnumerator CoroutineCommand ();
}
