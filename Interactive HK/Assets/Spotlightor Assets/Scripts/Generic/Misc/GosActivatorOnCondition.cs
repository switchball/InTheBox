using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GosActivatorOnCondition : MonoBehaviour
{
	public bool updateEachFrame = false;
	public List<GameObject> activeObjectsIfTrue;
	public List<GameObject> activeObjectsIfFalse;

	protected abstract bool HasMetCondition{ get; }

	protected virtual void Awake()
	{
		ActivateObjectsOnCondition ();
	}

	protected virtual void OnEnable ()
	{
		ActivateObjectsOnCondition ();
	}

	void Update ()
	{
		if (updateEachFrame)
			ActivateObjectsOnCondition ();
	}

	public void ActivateObjectsOnCondition ()
	{
		ActivateObjectsOnCondition (this.HasMetCondition);
	}

	[ContextMenu ("Activate Objs on TRUE")]
	public void ActivateObjectsOnConditionMet ()
	{
		ActivateObjectsOnCondition (true);
	}

	[ContextMenu ("Activate Objs on FALSE")]
	public void ActivateObjectsOnConditionNotMet ()
	{
		ActivateObjectsOnCondition (false);
	}

	private void ActivateObjectsOnCondition (bool hasMetCondition)
	{
		activeObjectsIfTrue.ForEach (obj => obj.SetActive (hasMetCondition));
		activeObjectsIfFalse.ForEach (obj => obj.SetActive (!hasMetCondition));
	}
}
