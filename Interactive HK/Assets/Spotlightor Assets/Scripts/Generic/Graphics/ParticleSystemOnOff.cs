using UnityEngine;
using System.Collections;

[System.Serializable]
public class ParticleSystemOnOff
{
	public ParticleSystem particleSystem;
	private bool isOn = false;

	public bool IsOn {
		get { return isOn; }
		set {
			if (isOn != value) {
				isOn = value;
				if (isOn)
					particleSystem.Play ();
				else
					particleSystem.Stop ();
			}
		}
	}
}
