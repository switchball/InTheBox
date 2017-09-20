using UnityEngine;
using System.Collections;

public class CappedIntArraySaver
{
	private int maxValue;
	private string key;
	private int[] values;

	public int ArraySize {
		get { return values.Length;}
	}

	public CappedIntArraySaver (int maxValue, int arraySize, string key)
	{
		this.maxValue = maxValue;
		this.key = key;
		values = new int[arraySize];
		
		Load ();
	}
	
	public void Load ()
	{
		int savedValue = BasicDataTypeStorage.GetInt (key);
		
		int divider = 1;
		for (int i = 0; i < ArraySize-1; i++)
			divider *= maxValue + 1;
		for (int i = 0; i < ArraySize; i++) {
			int decodedValue = savedValue / divider;
			values [ArraySize - i - 1] = decodedValue;
			
			savedValue -= decodedValue * divider;
			divider /= maxValue + 1;
		}
	}

	public int GetValueAt (int index)
	{
		return values [Mathf.Clamp (index, 0, ArraySize - 1)];
	}
	
	public void SetValueAt (int index, int newValue)
	{
		values [Mathf.Clamp (index, 0, ArraySize - 1)] = Mathf.Clamp (newValue, 0, maxValue);
	}
	
	public void Save ()
	{
		int saveValue = 0;
		int offset = 1;
		for (int i = 0; i < values.Length; i++) {
			saveValue += values [i] * offset;
			offset *= maxValue + 1;
		}
		BasicDataTypeStorage.SetInt (key, saveValue);
	}

	public void Clear ()
	{
		for (int i = 0; i < values.Length; i++)
			values [i] = 0;

		BasicDataTypeStorage.DeleteInt (key);
	}
}
