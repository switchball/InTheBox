using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomInputBehavior : MonoBehaviour
{
	void Update ()
	{
		foreach (KeyValuePair<string, CustomInputAxis> pair in CustomInput.InputAxisDictionary) {
			CustomInputAxis inputAxis = pair.Value;

			if (inputAxis.PossitiveKey != KeyCode.None && Input.GetKey (inputAxis.PossitiveKey))
				inputAxis.RawValue = 1;
			if (inputAxis.NegativeKey != KeyCode.None && Input.GetKey (inputAxis.NegativeKey))
				inputAxis.RawValue = -1;

			if (inputAxis.RawValueUpdated == false && inputAxis.Setting.altInputAxisName != "")
				inputAxis.RawValue = Input.GetAxisRaw (inputAxis.Setting.altInputAxisName);

			inputAxis.UpdateSmoothedValue ();
		}
	}
}
