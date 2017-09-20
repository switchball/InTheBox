using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XboxSinglePlayerEngager : MonoBehaviour
{
	public enum StateTypes
	{
		NotSet,
		Setup,
		Set,
		SwitchProfile,
		Watch,
		ReEngage,
	}

	private static ObjectInstanceFinder<XboxSinglePlayerEngager> instanceFinder = new ObjectInstanceFinder<XboxSinglePlayerEngager> ();

	public static XboxSinglePlayerEngager Instance{ get { return instanceFinder.Instance; } }

	public XboxEngager engager;
	public bool debug = false;

	private Fsm<XboxSinglePlayerEngager, StateTypes> finiteStateMachine;

	public Fsm<XboxSinglePlayerEngager, StateTypes> FiniteStateMachine {
		get {
			if (finiteStateMachine == null) {
				finiteStateMachine = new Fsm<XboxSinglePlayerEngager, StateTypes> (this);
				finiteStateMachine.AddStates (GetComponentsInChildren<XboxSinglePlayerEngagerState> (true));
				finiteStateMachine.GotoState (StateTypes.NotSet);
			}
			return finiteStateMachine;
		}
	}

	public XboxPlayer ActivePlayer {
		get { return XboxActivePlayers.First; }
	}

	void Awake ()
	{
		if (Instance == this) {
			if (transform.parent == null)
				DontDestroyOnLoad (gameObject);
			
			GotoState (StateTypes.NotSet);
		} else
			Destroy (gameObject);
	}

	public void GotoState (StateTypes stateType)
	{
		FiniteStateMachine.GotoState (stateType);
	}

	void OnGUI ()
	{
		if (debug) {
			GUI.Box (new Rect (30, 30, 500, 30), string.Format ("SinglePlayerEngager State: {0}", FiniteStateMachine.CurrentState.StateId));
			GUI.Box (new Rect (30, 60, 500, 30), string.Format ("ActivePlayer: {0}", ActivePlayer));
		}
	}
}
