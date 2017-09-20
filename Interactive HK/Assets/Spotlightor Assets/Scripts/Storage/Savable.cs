using UnityEngine;
using System.Collections;

public abstract class Savable<T>
{
	private string saveKey;
	private T defaultValue;

	public T Value {
		get {
			return HasBeenSavedWithKey (saveKey) ? Load (saveKey) : defaultValue;
		}
		set {
			Save (saveKey, value);
		}
	}

	public bool HasBeenSaved{ get { return this.HasBeenSavedWithKey (saveKey); } }

	public Savable (string saveKey, T defaultValue)
	{
		this.saveKey = saveKey;
		this.defaultValue = defaultValue;
	}

	public void Delete ()
	{
		this.Delete (saveKey);
	}

	protected abstract bool HasBeenSavedWithKey (string key);

	protected abstract T Load (string key);

	protected abstract void Save (string key, T value);

	protected abstract void Delete (string key);
}
