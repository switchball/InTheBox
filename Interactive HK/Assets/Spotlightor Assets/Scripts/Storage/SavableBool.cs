using UnityEngine;
using System.Collections;

public class SavableBool : Savable<bool>
{
	public SavableBool (string key, bool deaultValue) : base (key, deaultValue)
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

	protected override bool Load (string key)
	{
		return BasicDataTypeStorage.GetInt (key) > 0;
	}

	protected override void Save (string key, bool value)
	{
		BasicDataTypeStorage.SetInt (key, value == true ? 1 : 0);
	}

	public static implicit operator bool (SavableBool t)
	{
		return t.Value;
	}
}
