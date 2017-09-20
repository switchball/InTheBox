using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SaveSlotsStorage
{
	public const string SlotsResourcesPath = "Save/Slots";

	private static List<SaveSlot> slots;

	private static SaveSlot activeSaveSlot;

	public static List<SaveSlot> Slots { get { return slots; } }

	static SaveSlotsStorage ()
	{
		slots = new List<SaveSlot> (Resources.LoadAll<SaveSlot> (SlotsResourcesPath));
		if (slots.Count == 0) {
			Debug.LogWarning (string.Format ("Failed to load any SaveSlots at Resources path: {0}", SlotsResourcesPath));

			SaveSlot slot = ScriptableObject.CreateInstance<SaveSlot> ();
			slot.name = "default";
			slot.isDefault = true;
			slots.Add (slot);

			Debug.Log (string.Format ("Default SaveLost created: {0}", slot.name));
		}
	}

	public static SaveSlot ActiveSaveSlot {
		get {
			if (activeSaveSlot == null) {
				activeSaveSlot = slots.Find (s => s.isDefault);
				if (activeSaveSlot == null) {
					activeSaveSlot = slots [0];
					Debug.LogWarning (string.Format ("No SaveSlot is marked as isDefault. The 1st one:{0} will be picked", activeSaveSlot.name));
				}
			}
			return activeSaveSlot;
		}
		set {
			activeSaveSlot = value;
		}
	}

	public static SaveSlot GetSaveSlot (string slotName)
	{
		SaveSlot saveSlot = slots.Find (s => s.name == slotName);
		return saveSlot;
	}
}
