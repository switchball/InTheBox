using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Light))]
public class LightTransitionManager : ValueTransitionManager
{
	public float inIntensity = 1;
	public float outIntensity = 0;
	public bool autoEnableLight = true;

	protected override void OnProgressValueUpdated (float progress)
	{
		Light myLight = GetComponent<Light> ();

		myLight.intensity = Mathf.LerpUnclamped (outIntensity, inIntensity, progress);

		if (autoEnableLight)
			myLight.enabled = myLight.intensity > 0;
	}

	void Reset ()
	{
		Light myLight = GetComponent<Light> ();
		if (myLight != null)
			inIntensity = myLight.intensity;
	}
}
