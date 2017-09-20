using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class GameLevelInfoBase : ScriptableObject
{
	public bool active = true;
	private SavableInt completeTimes;

	public bool IsCompleted {
		get { return CompletedTimes.Value > 0; } 
		set {
			if (value)
				CompletedTimes.UpdateMax (1);
			else
				CompletedTimes.Value = 0;
		} 
	}

	public SavableInt CompletedTimes {
		get {
			if (completeTimes == null)
				completeTimes = new SavableInt (SaveKeyPrefix + "complete_times", 0);
			return completeTimes;
		}
	}

	public virtual string SceneName{ get { return this.name; } }

	protected virtual string SaveKeyPrefix{ get { return this.name; } }

	public abstract int LevelNumber{ get; }

	public abstract int WorldNumber{ get; }

	public abstract bool IsUnlocked{ get; }

	public void DeleteSaveData ()
	{
		OnDeleteSaveData ();
		CompletedTimes.Delete ();
	}

	protected abstract void OnDeleteSaveData ();
}
