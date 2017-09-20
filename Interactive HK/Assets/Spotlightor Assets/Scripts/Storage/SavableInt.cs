using UnityEngine;
using System.Collections;

public class SavableInt : Savable<int>
{
	public SavableInt (string key, int defaultValue):base(key, defaultValue)
	{
	}

	protected override void Delete (string key)
	{
		BasicDataTypeStorage.DeleteInt (key);
	}

	protected override bool HasBeenSavedWithKey (string key)
	{
		return BasicDataTypeStorage.HasKey (key);
	}

	protected override int Load (string key)
	{
		return BasicDataTypeStorage.GetInt (key);
	}

	protected override void Save (string key, int value)
	{
		BasicDataTypeStorage.SetInt (key, value);
	}

	public static implicit operator int (SavableInt t)
	{
		return t.Value;
	}

	public void UpdateMax (int value)
	{
		this.Value = Mathf.Max (this.Value, value);
	}
	
	public void UpdateMin (int value)
	{
		this.Value = Mathf.Min (this.Value, value);
	}
}
