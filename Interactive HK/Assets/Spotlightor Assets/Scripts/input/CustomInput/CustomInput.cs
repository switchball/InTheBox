using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CustomInput
{
	private static Dictionary<string, CustomInputAxis> inputAxisDictionary;
	private static CustomInputBehavior inputBehavior;

	public static Dictionary<string, CustomInputAxis> InputAxisDictionary { get { return inputAxisDictionary; } }

	public static CustomInputBehavior InputBehavior { get { return inputBehavior; } }

	static CustomInput ()
	{
		inputAxisDictionary = new Dictionary<string, CustomInputAxis> ();
		CustomInputSettingsAsset settingsAsset = Resources.Load (CustomInputSettingsAsset.ResourceAssetPath, typeof(CustomInputSettingsAsset)) as CustomInputSettingsAsset;
		if (settingsAsset != null) {
			foreach (CustomInputAxisSetting axisSetting in settingsAsset.inputAxisSettings) {
				inputAxisDictionary.Add (axisSetting.axisName, new CustomInputAxis (axisSetting));
			}
		} else 
			Debug.LogError (string.Format ("Cannot load CustomInputSettingsAsset at resource path: {0}", CustomInputSettingsAsset.ResourceAssetPath));

		GameObject inputBehaviorGo = new GameObject ("[CustomInputBehavior]");
		inputBehavior = inputBehaviorGo.AddComponent<CustomInputBehavior> ();
		GameObject.DontDestroyOnLoad (inputBehaviorGo);
		GameObject.DontDestroyOnLoad (inputBehavior);
	}

	public static float GetAxis (string axisName)
	{
		CustomInputAxis inputAxis = GetInputAxisByAxisName (axisName);
		if (inputAxis != null)
			return inputAxis.SmoothedValue;
		else
			return 0;
	}
	
	public static void SetAxisRawValue (string axisName, float rawValue)
	{
		CustomInputAxis inputAxis = GetInputAxisByAxisName (axisName);
		if (inputAxis != null)
			inputAxis.RawValue = rawValue;
	}
	
	public static CustomInputAxis GetInputAxisByAxisName (string axisName)
	{
		CustomInputAxis inputAxis;
		if (inputAxisDictionary.TryGetValue (axisName, out inputAxis) == false) {
			Debug.LogWarning ("Cannot find Custom Input Axis with name: " + axisName);
			inputAxis = null;
		}
		return inputAxis;
	}

	public static void UpdateAllAxisSmoothedValues ()
	{
		foreach (KeyValuePair<string, CustomInputAxis> inputSetting in inputAxisDictionary)
			inputSetting.Value.UpdateSmoothedValue ();
	}
}
