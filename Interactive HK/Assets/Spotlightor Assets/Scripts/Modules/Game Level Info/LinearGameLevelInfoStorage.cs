using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class LinearGameLevelInfoStorage<T>  where T:GameLevelInfoBase
{
	[System.NonSerialized]
	private List<T>
		infos;

	protected abstract string InfosResourcesPath{ get; }

	public T FinalLevelInfo{ get { return Infos [Infos.Count - 1]; } }

	public bool AllLevelsCompleted {
		get {
			bool allLevelsCompleted = true;
			for (int i = Infos.Count - 1; i >= 0; i--) {
				if (Infos [i].IsCompleted == false) {
					allLevelsCompleted = false;
					break;
				}
			}
			return allLevelsCompleted;
		}
	}

	public List<T> Infos {
		get {
			if (infos == null || infos.Contains (null))
				ReloadLevelInfos ();
			return infos;
		}
	}

	public T FindLevelInfoBySceneName (string sceneName)
	{
		return Infos.Find (info => info.SceneName == sceneName);
	}

	public T GetLevelInfo (int worldNumber, int levelNumber)
	{
		return Infos.Find (info => info.WorldNumber == worldNumber && info.LevelNumber == levelNumber);
	}

	public List<T> GetLevelInfosOfWorld (int worldNumber)
	{
		return Infos.FindAll (info => info.WorldNumber == worldNumber);
	}

	public T GetPrevLevelInfo (T levelInfo)
	{
		T result = null;

		int levelIndex = Infos.IndexOf (levelInfo);
		if (levelIndex >= 1)
			result = Infos [levelIndex - 1];

		return result;
	}

	public T GetNextLevelInfo (T levelInfo)
	{
		T result = null;
		
		int levelIndex = Infos.IndexOf (levelInfo);
		if (levelIndex >= 0 && levelIndex < Infos.Count - 1)
			result = Infos [levelIndex + 1];
		
		return result;
	}

	public virtual void CompleteAllLevels ()
	{
		Infos.ForEach (info => info.IsCompleted = true);
	}

	public void DeleteAllSaveData ()
	{
		Infos.ForEach (info => info.DeleteSaveData ());
	}

	public void ReloadLevelInfos ()
	{
		infos = new List<T> (Resources.LoadAll<T> (InfosResourcesPath));
		infos = infos.FindAll (info => info.active);
		infos.Sort ((x, y) => x.name.CompareTo (y.name));
	}
}
