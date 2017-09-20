using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SaveEngine : ScriptableObject
{
	public enum StateTypes
	{
		NotInstalled,
		Installing,
		Installed,
		Uninstalling,
	}

	public struct SlotOperationResult
	{
		public string slotName;
		public bool success;
		public string message;

		public SlotOperationResult (string slotName, bool success)
		{
			this.slotName = slotName;
			this.success = success;
			if (success)
				message = "Success";
			else
				message = "Fail";
		}
	}

	public delegate void GenerictEventHandler (SaveEngine engine);

	public delegate void StateChangedEventHandler (SaveEngine engine, StateTypes currentState, StateTypes prevState);

	public delegate void SlotOperationResultEventHandler (SaveEngine engine, SlotOperationResult result);

	public delegate void LoadCompletedEventHandler (SaveEngine engine, SlotOperationResult result, string loadedSaveDataString);

	public event GenerictEventHandler SaveStarted;
	public event StateChangedEventHandler StateChanged;
	public event SlotOperationResultEventHandler SaveCompleted;
	public event LoadCompletedEventHandler LoadCompleted;
	public event SlotOperationResultEventHandler DeleteCompleted;

	private StateTypes state = StateTypes.NotInstalled;

	public StateTypes State {
		get { return state; }
		protected set {
			if (state != value) {
				StateTypes prevState = this.state;
				state = value;
				if (StateChanged != null)
					StateChanged (this, state, prevState);
			}
		}
	}

	public void Install ()
	{
		if (State == StateTypes.NotInstalled) {
			this.Log ("Install SaveEngine {0}", name);
			State = StateTypes.Installing;
			DoInstall ();
		} else {
			this.Log ("Cannot install SaveEngine on State: {0}", State);
		}
	}

	public void Uninstall ()
	{
		if (State == StateTypes.Installed) {
			this.Log ("Uninstall SaveEngine {0}", name);
			State = StateTypes.Uninstalling;
			DoUninstall ();
		} else {
			this.Log ("Cannot uninstall SaveEngine on State: {0}", State);
		}
	}

	protected abstract void DoInstall ();

	protected abstract void DoUninstall ();

	public abstract void Save (string slotName, string saveDataString);

	public abstract void Load (string slotName);

	public abstract void Delete (string slotName);

	protected void OnInstallationCompleted (bool success)
	{
		if (success)
			State = StateTypes.Installed;
		else
			State = StateTypes.NotInstalled;
		this.Log ("Install {0} completed, success: {1}", name, success);
	}

	protected void OnUninstallationCompleted (bool success)
	{
		if (success)
			State = StateTypes.NotInstalled;
		else
			State = StateTypes.Installed;
		this.Log ("Uninstall {0} completed, success: {1}", name, success);
	}

	protected void OnSaveStarted ()
	{
		if (SaveStarted != null)
			SaveStarted (this);
	}

	protected void OnSaveCompleted (SlotOperationResult result)
	{
		if (SaveCompleted != null)
			SaveCompleted (this, result);
	}

	protected void OnLoadCompleted (SlotOperationResult result, string loadedSaveDataString)
	{
		if (LoadCompleted != null)
			LoadCompleted (this, result, loadedSaveDataString);
	}

	protected void OnDeleteCompleted (SlotOperationResult result)
	{
		if (DeleteCompleted != null)
			DeleteCompleted (this, result);
	}
}
