using UnityEngine;
using System.Collections;

public class CustomInputSettingsAsset : ScriptableObject
{
	public const string ResourceAssetPath = "custom_input_setting";
	public CustomInputAxisSetting[] inputAxisSettings;
}
