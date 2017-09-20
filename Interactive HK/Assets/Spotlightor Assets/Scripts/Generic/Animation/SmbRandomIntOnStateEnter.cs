using UnityEngine;
using System.Collections;

#if UNITY_5
public class SmbRandomIntOnStateEnter : StateMachineBehaviour
{
	[Header ("Inclusive")]
	public int min = 0;
	[Header ("Exclusive")]
	public int max = 3;
	public string intName = "IdleType";

	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetInteger (intName, Random.Range (min, max));
	}
}
#endif
