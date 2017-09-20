using UnityEngine;
using System.Collections;

#if UNITY_5
public class RandomFloatOnStateEnter : StateMachineBehaviour
{
	public float min = 0;
	public float max = 3;
	public bool useIntValue = true;
	[SingleLineLabel ()]
	public string
		floatName = "IdleType";

	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		float randomValue = useIntValue ? Random.Range ((int)min, (int)max + 1) : Random.Range (min, max);
		animator.SetFloat (floatName, randomValue);
	}
}
#endif
