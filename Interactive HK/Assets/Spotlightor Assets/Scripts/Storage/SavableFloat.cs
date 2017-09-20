using UnityEngine;
using System.Collections;

public class SavableFloat : Savable<float>
{
	public SavableFloat (string key, float defaultValue):base(key, defaultValue)
	{
	}

	public void UpdateMax (float value)
	{
		this.Value = Mathf.Max (this.Value, value);
	}

	public void UpdateMin (float value)
	{
		this.Value = Mathf.Min (this.Value, value);
	}

	protected override void Delete (string key)
	{
		BasicDataTypeStorage.DeleteFloat (key);
	}

	protected override bool HasBeenSavedWithKey (string key)
	{
		return BasicDataTypeStorage.HasKey (key);
	}

	protected override float Load (string key)
	{
		return BasicDataTypeStorage.GetFloat (key);
	}

	protected override void Save (string key, float value)
	{
		BasicDataTypeStorage.SetFloat (key, value);
	}

	public static implicit operator float (SavableFloat t)
	{
		return t.Value;
	}

}
