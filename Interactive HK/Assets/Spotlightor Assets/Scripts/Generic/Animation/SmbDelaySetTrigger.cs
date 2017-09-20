using UnityEngine;
using System.Collections;

#if UNITY_5_3
public class SmbDelaySetTrigger : StateMachineBehaviour
{
	public RandomRangeFloat delayTimeRange = new RandomRangeFloat (3, 5);
	public string triggerName = "";
	private float delayTime = 3;
	private float enterTime = 0;
	private bool triggered = false;

	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		delayTime = delayTimeRange.RandomValue;
		enterTime = Time.time;
		triggered = false;
	}

	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		float timeElapsed = Time.time - enterTime;

		if (timeElapsed >= delayTime && triggered == false) {
			animator.SetTrigger (triggerName);
			triggered = true;
		}
	}
}
#else
public class SmbDelaySetTrigger
{
}
#endif