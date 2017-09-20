using UnityEngine;
using System.Collections;

public class SaveSlotSaveCommand : CoroutineCommandBehavior
{
	public bool autoInstallSaveEngine = true;

	protected override IEnumerator CoroutineCommand ()
	{
		if (HybridSaveEngine.Instance.State != SaveEngine.StateTypes.Installed && autoInstallSaveEngine) {
			this.Log ("Save while notInstalled, autoInstall save engine");
			GameObject loaderGo = new GameObject ("Load Save Data");
			SaveSlotLoadCommand loader = loaderGo.AddComponent<SaveSlotLoadCommand> ();
			yield return StartCoroutine (loader.ExecuteCoroutine ());
		}

		this.Log ("Save game");

		if (HybridSaveEngine.Instance.State == SaveEngine.StateTypes.Installed) {
			SaveSlotsStorage.ActiveSaveSlot.Save ();
			yield return null;
		}
	}
}
