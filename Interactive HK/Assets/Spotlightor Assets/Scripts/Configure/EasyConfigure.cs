using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO Configure may be changed while playing, and then the configure value users should be informed.
// Use EasyConfigure.GetConfigureFloat(key) to return a unique RangeFloat instance, and value users could register listener
// Or, use a event handlers to fire events after modifiying the configure values
public static class EasyConfigure
{
	private static Dictionary<string, float> floatConfigureDict = new Dictionary<string, float> ();
	private static Dictionary<string, int> intConfigureDict = new Dictionary<string, int> ();
	private static Dictionary<string, string> stringConfigureDict = new Dictionary<string, string> ();

	public static float GetFloatWithDefaultValue (string key, float defaultValue)
	{
		float value = defaultValue;
		if (floatConfigureDict.TryGetValue (key, out value) == false) {
			if (BasicDataTypeStorage.HasKey (key)) 
				value = BasicDataTypeStorage.GetFloat (key);
			else
				value = defaultValue;

			floatConfigureDict [key] = value;

			SaveFloat(key);
		}
		return value;
	}

	public static float GetFloat (string key)
	{
		float value = 0;
		if (floatConfigureDict.TryGetValue (key, out value) == false) {
			if (BasicDataTypeStorage.HasKey (key)) {
				value = BasicDataTypeStorage.GetFloat (key);
				floatConfigureDict [key] = value;
			}
		}
		return value;
	}

	public static void SetFloat (string key, float value)
	{
		floatConfigureDict [key] = value;
	}

	public static void SaveFloat (string key)
	{
		if (floatConfigureDict.ContainsKey (key))
			BasicDataTypeStorage.SetFloat (key, floatConfigureDict [key]);
	}

	public static int GetIntWithDefaultValue (string key, int defaultValue)
	{
		int value = defaultValue;
		if (intConfigureDict.TryGetValue (key, out value) == false) {
			if (BasicDataTypeStorage.HasKey (key))
				value = BasicDataTypeStorage.GetInt (key);
			else 
				value = defaultValue;

			intConfigureDict [key] = value;

			SaveInt (key);
		}
		return value;
	}

	public static int GetInt (string key)
	{
		int value = 0;
		if (intConfigureDict.TryGetValue (key, out value) == false) {
			if (BasicDataTypeStorage.HasKey (key)) {
				value = BasicDataTypeStorage.GetInt (key);
				intConfigureDict [key] = value;
			}
		}
		return value;
	}
	
	public static void SetInt (string key, int value)
	{
		intConfigureDict [key] = value;
	}
	
	public static void SaveInt (string key)
	{
		if (intConfigureDict.ContainsKey (key))
			BasicDataTypeStorage.SetInt (key, intConfigureDict [key]);
	}

	public static string GetStringWithDefaultValue (string key, string defaultValue)
	{
		string value = defaultValue;
		if (stringConfigureDict.TryGetValue (key, out value) == false) {
			if (BasicDataTypeStorage.HasKey (key))
				value = BasicDataTypeStorage.GetString (key);
			else
				value = defaultValue;

			stringConfigureDict [key] = value;

			SaveString(key);
		}
		return value;
	}

	public static string GetString (string key)
	{
		string value = "";
		if (stringConfigureDict.TryGetValue (key, out value) == false) {
			if (BasicDataTypeStorage.HasKey (key)) {
				value = BasicDataTypeStorage.GetString (key);
				stringConfigureDict [key] = value;
			}else value = "";
		}
		return value;
	}
	
	public static void SetString (string key, string value)
	{
		stringConfigureDict [key] = value;
	}
	
	public static void SaveString (string key)
	{
		if (stringConfigureDict.ContainsKey (key))
			BasicDataTypeStorage.SetString (key, stringConfigureDict [key]);
	}
}
