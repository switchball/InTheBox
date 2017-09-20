using UnityEngine;
using System.Collections;

public class WaitForAnimatorStateCommand : CoroutineCommandBehavior
{
	public Animator animator;
	public int layerIndex = 0;
	public string stateName;
	[Range(0,1)]
	public float
		normalizedTime = 1;

	protected override IEnumerator CoroutineCommand ()
	{
		while (animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName) == false 
		      || animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime < normalizedTime)
			yield return null;
	}
}
