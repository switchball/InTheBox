using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_XBOXONE
using Users;
#endif

[RequireComponent (typeof(XboxEngagerStateIdle))]
[RequireComponent (typeof(XboxEngagerStateController))]
[RequireComponent (typeof(XboxEngagerStateUser))]
public class XboxEngager : MonoBehaviour
{
	public enum StateTypes
	{
		Idle,
		EngageController,
		EngageUser,
	}

	public abstract class EngageState : SimpleFsmStateMonobehavior<XboxEngager, StateTypes>
	{
		protected void GotoState (StateTypes stateType)
		{
			Owner.FiniteStateMachine.GotoState (stateType);
		}
	}

	public delegate void GenericEventHandler (XboxEngager engager);

	public event GenericEventHandler ControllerEngageStarted;
	public event GenericEventHandler ControllerEngageCompleted;
	public event GenericEventHandler Completed;

	private static ObjectInstanceFinder<XboxEngager> instanceFinder = new ObjectInstanceFinder<XboxEngager> ();

	public static XboxEngager Instance {
		get { return instanceFinder.Instance; }
	}

	private Fsm<XboxEngager, StateTypes> finiteStateMachine;

	public bool IsEngaging {
		get { return FiniteStateMachine.CurrentState.StateId != StateTypes.Idle; }
	}

	private XboxEngagerStateController EngageControllerState {
		get { return  FiniteStateMachine.GetState (StateTypes.EngageController) as XboxEngagerStateController; }
	}

	private XboxEngagerStateUser EngageUserState {
		get { return  FiniteStateMachine.GetState (StateTypes.EngageUser) as XboxEngagerStateUser; }
	}

	public Fsm<XboxEngager, StateTypes> FiniteStateMachine {
		get {
			if (finiteStateMachine == null) {
				finiteStateMachine = new Fsm<XboxEngager, StateTypes> (this);
				finiteStateMachine.AddStates (GetComponentsInChildren<EngageState> (true));
				finiteStateMachine.GotoState (StateTypes.Idle);
				finiteStateMachine.StateBegin += HandleStateBegin;
				finiteStateMachine.StateEnd += HandleStateEnd;
				finiteStateMachine.StateChange += HandleStateChange;
			}
			return finiteStateMachine;
		}
	}

	public bool IsEngagedControllerUserPairValid {
		get {
			bool isPairValid = false;
			#if UNITY_XBOXONE
			ulong controllerId = EngagedControllerId;
			if (IsValidGamepad (controllerId)) {
				uint joystickId = XboxOneInput.GetJoystickId (controllerId);
				int controllerPairedUserId = XboxOneInput.GetUserIdForGamepad (joystickId);
				int userId = EngagedUserId;
				if (userId == controllerPairedUserId) {
					User user = UsersManager.FindUserById (userId);
					if (user != null && user.IsSignedIn)
						isPairValid = true;
				}
			}
			#endif
			return isPairValid;
		}
	}

	public ulong EngagedControllerId { 
		get {
			ulong controllerId = 0;
			XboxEngagerStateController controllerState = FiniteStateMachine.GetState (StateTypes.EngageController) as XboxEngagerStateController;
			if (controllerState != null)
				controllerId = controllerState.EngagedControllerId;
			return controllerId;
		} 
	}

	public int EngagedUserId {
		get {
			int userId = -1;
			XboxEngagerStateUser userState = FiniteStateMachine.GetState (StateTypes.EngageUser) as XboxEngagerStateUser;
			if (userState != null)
				userId = userState.EngagedUserId;
			return userId;
		}
	}

	#if UNITY_XBOXONE
	public static bool IsValidGamepad (ulong controllerId)
	{
		uint joystickId = XboxOneInput.GetJoystickId (controllerId);
		return joystickId > 0 && XboxOneInput.IsGamepadActive (joystickId);
	}
	#endif

	public void Engage ()
	{
		EngageForController (0);
	}

	public void EngageForController (ulong targetControllerId)
	{
		EngageForControllerAndPlayer (targetControllerId, -1);
	}

	public void EngageForUser (int targetUserId)
	{
		EngageForControllerAndPlayer (0, targetUserId);
	}

	public void EngageForControllerAndPlayer (ulong targetControllerId, int targetUserId)
	{
		if (IsEngaging == false) {
			EngageControllerState.TargetControllerId = targetControllerId;

			EngageUserState.TargetUserId = targetUserId;
			EngageUserState.ForceRequestUserSignIn = false;

			FiniteStateMachine.GotoState (StateTypes.EngageController);
		} else
			this.LogWarning ("Cannot Engage while IsEngaging");
	}

	public void SwitchProfile (ulong controllerId)
	{
		if (IsEngaging == false) {
			EngageControllerState.EngagedControllerId = controllerId;
			EngageUserState.ControllerId = controllerId;
			EngageUserState.ForceRequestUserSignIn = true;

			FiniteStateMachine.GotoState (StateTypes.EngageUser);
		} else
			this.LogWarning ("Cannot SwitchProfile while IsEngaging");
	}

	private void HandleStateBegin (StateTypes stateId)
	{
		if (stateId == StateTypes.EngageController) {
			if (ControllerEngageStarted != null)
				ControllerEngageStarted (this);
		}
	}

	private void HandleStateEnd (StateTypes stateId)
	{
		if (stateId == StateTypes.EngageController) {
			EngageUserState.ControllerId = EngageControllerState.EngagedControllerId;
		}
	}

	void HandleStateChange (StateTypes newStateId, StateTypes previousStateId)
	{
		if (newStateId == StateTypes.EngageUser && previousStateId == StateTypes.EngageController) {
			if (ControllerEngageCompleted != null)
				ControllerEngageCompleted (this);
		} else if (newStateId == StateTypes.Idle && previousStateId == StateTypes.EngageUser) {
			if (Completed != null)
				Completed (this);
		}
	}


}
