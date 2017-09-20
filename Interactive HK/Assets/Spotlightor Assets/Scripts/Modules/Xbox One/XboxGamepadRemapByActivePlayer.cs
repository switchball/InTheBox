using UnityEngine;
using System.Collections;

public class XboxGamepadRemapByActivePlayer : MonoBehaviour
{
	private const int XboxMaxGamepadsCount = 8;
	public uint playerIndex = 0;
	public uint activeJoystickNum = 1;
	public uint inactiveJoystickNum = 2;
	[SingleLineLabel ()]
	public bool activeAllIfNoController = true;

	private XboxPlayer Player{ get { return XboxActivePlayers.Players [(int)playerIndex]; } }

	#if UNITY_XBOXONE
	void OnEnable ()
	{
		if (Application.platform == RuntimePlatform.XboxOne) {
			RemapJoysticks (Player.ControllerId);
			Player.ControllerChanged += HandleActivePlayerControllerChanged;
		}
	}

	void OnDisable ()
	{
		if (Application.platform == RuntimePlatform.XboxOne) {
			RestoreJoystickMapings ();
			Player.ControllerChanged -= HandleActivePlayerControllerChanged;
		}
	}

	void HandleActivePlayerControllerChanged (XboxPlayer xboxPlayer, ulong newControllerId, ulong oldControllerId)
	{
		RemapJoysticks (newControllerId);
	}

	private void RemapJoysticks (ulong activeControllerId)
	{
		if (activeControllerId > 0) {
			this.Log ("Active joystick mapped to Controller {0}(cid)", activeControllerId);
			for (uint joystickId = 1; joystickId <= XboxMaxGamepadsCount; joystickId++) {
				ulong controllerId = XboxOneInput.GetControllerId (joystickId);
				RemapController (controllerId, controllerId == activeControllerId);
			}

		} else {
			this.Log ("No active controller. All joystick mapped to {0} index", activeAllIfNoController ? "Active" : "Inactive");
			for (uint joystickId = 1; joystickId <= XboxMaxGamepadsCount; joystickId++)
				RemapController (XboxOneInput.GetControllerId (joystickId), activeAllIfNoController);
		}

		Input.ResetInputAxes ();
	}

	private void RemapController (ulong controllerId, bool isGamepadActive)
	{
		if (controllerId > 0) {
			uint joystickId = XboxOneInput.GetJoystickId (controllerId);
			if (joystickId > 0) {
				uint newJoystickId = isGamepadActive ? activeJoystickNum : inactiveJoystickNum;
				XboxOneInput.RemapGamepadToIndex (joystickId, newJoystickId);
//				this.Log ("Controller {0}(cid) remapped to joystickId: {1}({2})", controllerId, newJoystickId, isGamepadActive ? "ACTIVE" : "inactive");
			}
		}
	}

	private void RestoreJoystickMapings ()
	{
		for (uint joystickId = 1; joystickId <= XboxMaxGamepadsCount; joystickId++)
			XboxOneInput.RemapGamepadToIndex (joystickId, joystickId);

		Input.ResetInputAxes ();

		this.Log ("Joystick mapping restored");
	}

	#endif
}
