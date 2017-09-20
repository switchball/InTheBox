using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TempSaveEngine : SaveEngine
{
	private Dictionary<string, string> dataStringDict = new Dictionary<string, string> ();

	public override void Save (string slotName, string saveDataString)
	{
		OnSaveStarted ();

		dataStringDict [slotName] = saveDataString;

		SlotOperationResult result = new SlotOperationResult (slotName, true);
		OnSaveCompleted (result);
	}

	public override void Load (string slotName)
	{
		string loadedSaveDataString = "";
		SlotOperationResult result;
		if (dataStringDict.ContainsKey (slotName)) {
			loadedSaveDataString = dataStringDict [slotName];
			result = new SlotOperationResult (slotName, true);
		} else {
			result = new SlotOperationResult (slotName, false);
			result.message = "TempSaveEngine Dictionary doesn't has key: " + slotName;
		}

		OnLoadCompleted (result, loadedSaveDataString);
	}

	public override void Delete (string slotName)
	{
		SlotOperationResult result;
		if (dataStringDict.ContainsKey (slotName)) {
			dataStringDict.Remove (slotName);
			result = new SlotOperationResult (slotName, true);
		} else {
			result = new SlotOperationResult (slotName, false);
			result.message = "TempSaveEngine Dictionary doesn't has key: " + slotName;
		}

		OnDeleteCompleted (result);
	}

	protected override void DoInstall ()
	{
		OnInstallationCompleted (true);
	}

	protected override void DoUninstall ()
	{
		OnUninstallationCompleted (true);
	}
}
