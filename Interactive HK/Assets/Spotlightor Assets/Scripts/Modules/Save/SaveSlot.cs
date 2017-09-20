using UnityEngine;
using System.Collections;

public class SaveSlot : ScriptableObject
{
	public delegate void SlotOperationResult (SaveSlot saveSlot,bool success,string message);

	public event SlotOperationResult LoadCompleted;
	public event SlotOperationResult SaveCompleted;
	public event SlotOperationResult DeleteCompleted;

	public bool isDefault = false;

	[System.NonSerialized]
	private SaveData data = null;

	public SaveData Data { 
		get {
			if (data == null)
				data = SaveData.Empty;
			return data; 
		} 
	}

	public void Load ()
	{
		HybridSaveEngine.Instance.LoadCompleted += HandleSaveEngineLoadCompleted;
		HybridSaveEngine.Instance.Load (this.name);
	}

	public void Save ()
	{
		HybridSaveEngine.Instance.SaveCompleted += HandleSaveEngineSaveCompleted;
		HybridSaveEngine.Instance.Save (this.name, JsonUtility.ToJson (data));
	}

	public void Clear ()
	{
		Data.Clear ();
	}

	public void Delete ()
	{
		HybridSaveEngine.Instance.DeleteCompleted += HandleSaveEngineDeleteCompleted;
		HybridSaveEngine.Instance.Delete (this.name);
	}

	void HandleSaveEngineLoadCompleted (SaveEngine engine, SaveEngine.SlotOperationResult result, string loadedSaveDataString)
	{
		if (result.slotName == this.name) {
			HybridSaveEngine.Instance.LoadCompleted -= HandleSaveEngineLoadCompleted;

			if (result.success) {
				JsonUtility.FromJsonOverwrite (loadedSaveDataString, Data);
				this.Log ("SaveSlot {0} updated with successfully loaded save data string.", this.name);
			} else {
				Clear ();
				this.Log ("SaveSlot {0} cleared because loading faild.", this.name);
			}
			
			if (LoadCompleted != null)
				LoadCompleted (this, result.success, result.message);
		}
	}

	void HandleSaveEngineSaveCompleted (SaveEngine engine, SaveEngine.SlotOperationResult result)
	{
		if (result.slotName == this.name) {
			HybridSaveEngine.Instance.SaveCompleted -= HandleSaveEngineSaveCompleted;

			if (SaveCompleted != null)
				SaveCompleted (this, result.success, result.message);
		}
	}

	void HandleSaveEngineDeleteCompleted (SaveEngine engine, SaveEngine.SlotOperationResult result)
	{
		if (result.slotName == this.name) {
			HybridSaveEngine.Instance.DeleteCompleted -= HandleSaveEngineDeleteCompleted;

			if (DeleteCompleted != null)
				DeleteCompleted (this, result.success, result.message);
		}
	}

}
