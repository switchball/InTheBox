using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
	[System.Serializable]
	public class StringIntDictionary : SerializableDictionary<string, int>
	{
		
	}

	[System.Serializable]
	public class StringFloatDictionary : SerializableDictionary<string, float>
	{

	}

	[System.Serializable]
	public class StringStringDictionary : SerializableDictionary<string, string>
	{

	}

	[SerializeField]
	private StringIntDictionary ints;
	[SerializeField]
	private StringFloatDictionary floats;
	[SerializeField]
	private StringStringDictionary strings;

	public Dictionary<string, int> Ints{ get { return ints.Dictionary; } }

	public Dictionary<string, float> Floats{ get { return floats.Dictionary; } }

	public Dictionary<string, string> Strings{ get { return strings.Dictionary; } }

	public static SaveData Empty {
		get {
			SaveData saveData = new SaveData ();
			saveData.Clear ();
			return saveData;
		}
	}

	public virtual void Clear ()
	{
		JsonUtility.FromJsonOverwrite (JsonUtility.ToJson (new SaveData ()), this);
	}
}
