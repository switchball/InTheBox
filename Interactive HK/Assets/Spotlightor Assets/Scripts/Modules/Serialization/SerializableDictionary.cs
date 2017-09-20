using UnityEngine;
using System.Collections;
using System .Collections.Generic;

public class SerializableDictionary<T,U> : ISerializationCallbackReceiver
{
	public List<T> keys;
	public List<U> values;

	public Dictionary<T,U> Dictionary{ get; private set; }

	public virtual void OnBeforeSerialize ()
	{
		if (Application.isEditor && Application.isPlaying == false)
			ValidateKeyValuePairs ();
		else
			FillKeyValuesByDictionary ();
	}

	private void FillKeyValuesByDictionary ()
	{
		keys = new List<T> ();
		values = new List<U> ();

		if (Dictionary != null) {
			foreach (KeyValuePair<T,U> kvp in Dictionary) {
				keys.Add (kvp.Key);
				values.Add (kvp.Value);
			}
		}
	}

	private void ValidateKeyValuePairs ()
	{
		if (keys != null && values != null) {
			while (values.Count != keys.Count) {
				if (values.Count > keys.Count)
					values.RemoveAt (values.Count - 1);
				else
					values.Add (default(U));
			}
		}
	}

	public virtual void OnAfterDeserialize ()
	{
		Dictionary = new Dictionary<T, U> ();
		int keyValuePairCount = Mathf.Min (keys.Count, values.Count);

		for (int i = 0; i < keyValuePairCount; i++) {
			T key = keys [i];
			U value = values [i];

			Dictionary.Add (key, value);
		}
	}
}
