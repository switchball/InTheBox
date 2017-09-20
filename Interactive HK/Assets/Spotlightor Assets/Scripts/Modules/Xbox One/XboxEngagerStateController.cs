using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class XboxEngagerStateController : XboxEngager.EngageState
{
	public override XboxEngager.StateTypes StateId { get { return XboxEngager.StateTypes.EngageController; } }

	#if UNITY_XBOXONE
	public List<XboxOneKeyCode> controllerEngageKeys = new List<XboxOneKeyCode> () {
		XboxOneKeyCode.Gamepad1ButtonA,
		XboxOneKeyCode.Gamepad2ButtonA,
		XboxOneKeyCode.Gamepad3ButtonA,
		XboxOneKeyCode.Gamepad4ButtonA,
		XboxOneKeyCode.Gamepad5ButtonA,
		XboxOneKeyCode.Gamepad6ButtonA,
		XboxOneKeyCode.Gamepad7ButtonA,
		XboxOneKeyCode.Gamepad8ButtonA,
	};
	#endif

	public UnityEvent onControllerEngage;

	private ulong targetControllerId = 0;
	private ulong engagedControllerId = 0;

	public ulong TargetControllerId {
		get { return targetControllerId; }
		set { targetControllerId = value; }
	}

	public ulong EngagedControllerId {
		get { return engagedControllerId; }
		set { engagedControllerId = value; }
	}

	public override void BeginState (XboxEngager.StateTypes previousState)
	{
		base.BeginState (previousState);

		if (Application.platform == RuntimePlatform.XboxOne) {
			this.engagedControllerId = 0;
			this.Log ("Controller Engagement Start. TargetControllerId: {0}", TargetControllerId);
		} else
			GotoState (XboxEngager.StateTypes.EngageUser);
	}

	#if UNITY_XBOXONE
	void Update ()
	{
		if (engagedControllerId == 0) {
			foreach (XboxOneKeyCode key in controllerEngageKeys) {
				if (XboxOneInput.GetKeyDown (key)) {
					uint keyDownJoystickId = XboxOneInput.GetGamepadIndexFromGamepadButton (key);
					ulong keyDownControllerId = XboxOneInput.GetControllerId (keyDownJoystickId);

					bool engageSuccess = true;
					if (targetControllerId > 0 && XboxEngager.IsValidGamepad (targetControllerId))
						engageSuccess = (targetControllerId == keyDownControllerId);

					if (engageSuccess) {
						this.engagedControllerId = keyDownControllerId;
						this.Log ("XboxOne Key {0} down, EngagedControllerId: {1}, TargetControllerId: {2}", key, EngagedControllerId, TargetControllerId);

						onControllerEngage.Invoke ();

						GotoState (XboxEngager.StateTypes.EngageUser);
						break;
					}
				}
			}
		}
	}
	#endif
}
