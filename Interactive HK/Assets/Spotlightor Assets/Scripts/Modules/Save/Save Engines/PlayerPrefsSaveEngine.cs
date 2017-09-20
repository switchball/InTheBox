using UnityEngine;
using System.Collections;

public class PlayerPrefsSaveEngine : SaveEngine
{
	public override void Save (string slotName, string saveDataString)
	{
		OnSaveStarted ();

		PlayerPrefs.SetString (slotName, saveDataString);

		SlotOperationResult result = new SlotOperationResult (slotName, true);
		OnSaveCompleted (result);
	}

	public override void Load (string slotName)
	{
		string loadedSaveDataString = "";
		SlotOperationResult result;
		if (PlayerPrefs.HasKey (slotName)) {
			loadedSaveDataString = PlayerPrefs.GetString (slotName);
			result = new SlotOperationResult (slotName, true);
		} else {
			result = new SlotOperationResult (slotName, false);
			result.message = "PlayerPrefs doesn't has key: " + slotName;
		}

		OnLoadCompleted (result, loadedSaveDataString);
	}

	public override void Delete (string slotName)
	{
		SlotOperationResult result;
		if (PlayerPrefs.HasKey (slotName)) {
			PlayerPrefs.DeleteKey (slotName);
			result = new SlotOperationResult (slotName, true);
		} else {
			result = new SlotOperationResult (slotName, false);
			result.message = "PlayerPrefs doesn't has key: " + slotName;
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
