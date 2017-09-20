using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class XboxSinglePlayerEngagerStateEventTrigger : MonoBehaviour
{
	public XboxSinglePlayerEngager.StateTypes stateType;
	public UnityEvent onEnableWith;
	public UnityEvent onBegin;
	public UnityEvent onEnd;

	void OnEnable ()
	{
		if (Application.platform == RuntimePlatform.XboxOne) {
			if (XboxSinglePlayerEngager.Instance.FiniteStateMachine.CurrentState.StateId == stateType)
				onEnableWith.Invoke ();
		
			XboxSinglePlayerEngager.Instance.FiniteStateMachine.StateBegin += HandleStateBegin;
			XboxSinglePlayerEngager.Instance.FiniteStateMachine.StateEnd += HandleStateEnd;
		}
	}

	void OnDisable ()
	{
		if (Application.platform == RuntimePlatform.XboxOne) {
			XboxSinglePlayerEngager.Instance.FiniteStateMachine.StateBegin -= HandleStateBegin;
			XboxSinglePlayerEngager.Instance.FiniteStateMachine.StateEnd -= HandleStateEnd;
		}
	}

	void HandleStateBegin (XboxSinglePlayerEngager.StateTypes stateId)
	{
		if (stateId == stateType)
			onBegin.Invoke ();
	}

	void HandleStateEnd (XboxSinglePlayerEngager.StateTypes stateId)
	{
		if (stateId == stateType)
			onEnd.Invoke ();
	}
}
