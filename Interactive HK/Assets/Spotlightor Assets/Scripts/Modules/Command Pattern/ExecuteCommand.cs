using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CommandBehavior))]
public class ExecuteCommand : FunctionalMonoBehaviour
{

	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		GetComponent<CommandBehavior> ().Execute ();
	}

	protected override void OnBecameUnFunctional ()
	{

	}
}
