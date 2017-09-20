using UnityEngine;
using System.Collections;

public class CustomInputAxis
{
	public const float SensitivityScale = 2f;
	public const float SensitivityOffset = 0;
	public class ConfigurableKeyCode
	{
		private KeyCode defaultKey = KeyCode.Space;
		private string prefsSaveKey;
		private KeyCode key;
		
		public KeyCode DefaultKey { get { return defaultKey; } }
		
		public KeyCode Key {
			get { return key; }
			set {
				this.key = value;
				if (value != defaultKey) 
					BasicDataTypeStorage.SetInt (prefsSaveKey, (int)value);
				else 
					BasicDataTypeStorage.DeleteInt (prefsSaveKey);
			}
		}
		
		public ConfigurableKeyCode (KeyCode defaultKey, string prefsSaveKey)
		{
			this.defaultKey = defaultKey;
			this.prefsSaveKey = prefsSaveKey;
			key = BasicDataTypeStorage.HasKey (prefsSaveKey) ? (KeyCode)BasicDataTypeStorage.GetInt (prefsSaveKey) : this.defaultKey;
		}
	}
	private CustomInputAxisSetting setting;
	private ConfigurableKeyCode possitiveKeyConfigure;
	private ConfigurableKeyCode negativeKeyConfigure;
	private float smoothedValue = 0;
	private float rawValue = 0;
	private bool rawValueUpdated = false;

	public CustomInputAxisSetting Setting { get { return setting; } }

	public KeyCode PossitiveKey {
		get { return possitiveKeyConfigure.Key;}
		set { possitiveKeyConfigure.Key = value;}
	}

	public KeyCode NegativeKey {
		get { return negativeKeyConfigure.Key;}
		set { negativeKeyConfigure.Key = value;}
	}

	public CustomInputAxis (CustomInputAxisSetting setting)
	{
		this.setting = setting;
		this.possitiveKeyConfigure = new ConfigurableKeyCode (setting.defaultPossitiveKey, setting.axisName + "_p_key");
		this.negativeKeyConfigure = new ConfigurableKeyCode (setting.defaultNegativeKey, setting.axisName + "_n_key");
	}

	public float SmoothedValue {
		get { return smoothedValue;}
	}
	
	public float RawValue {
		set { 
			rawValue = value;
			rawValueUpdated = true;
		}
	}

	public bool RawValueUpdated {
		get { return rawValueUpdated; }
	}
	
	public void UpdateSmoothedValue ()
	{
		if (rawValue == 0) {
			if (smoothedValue != 0) 
				ChangeSmoothedValueTo (0, setting.gravity);
		} else {
			if (setting.snap) {
				if (rawValue >= 0 && smoothedValue < 0 || rawValue <= 0 && smoothedValue > 0)
					smoothedValue = 0;
			}
			if (smoothedValue != rawValue)
				ChangeSmoothedValueTo (rawValue, setting.sensitivity);
		}
		rawValue = 0;
		rawValueUpdated = false;
	}

	private void ChangeSmoothedValueTo (float targetValue, float speed)
	{
		float deltaValue = speed * Time.deltaTime;
		float distanceToTarget = Mathf.Abs (smoothedValue - targetValue);

		if (distanceToTarget < Mathf.Abs (deltaValue))
			deltaValue = distanceToTarget;
		if (smoothedValue < targetValue)
			smoothedValue += deltaValue;
		else
			smoothedValue -= deltaValue;
		
		if (Mathf.Abs (smoothedValue - targetValue) <= setting.dead)
			smoothedValue = targetValue;
	}
}
