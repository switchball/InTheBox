using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShakeSensor : MonoBehaviour
{
	public List<ShakeChannel> channels;
	public List<ShakeSource> targets;
	private List<ShakeSource> sensedShakeSources = new List<ShakeSource> ();

	public Vector3 ShakeIntensity{ get; private set; }

	public List<ShakeSource> SensedShakeSources { get { return sensedShakeSources; } }

	void Update ()
	{
		if (targets != null && targets.Count > 0)
			sensedShakeSources = targets;
		else {
			sensedShakeSources.Clear ();
			foreach (ShakeSource source in ShakeSource.globalInstances) {
				if (channels.Contains (source.channel))
					sensedShakeSources.Add (source);
			}
		}

		ShakeIntensity = Vector3.zero;
		foreach (ShakeSource source in sensedShakeSources)
			ShakeIntensity += source.GetShakeIntensityAtPosition (transform.position);
	}
}
