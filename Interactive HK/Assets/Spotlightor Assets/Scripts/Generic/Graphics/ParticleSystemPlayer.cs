using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ParticleSystemPlayer : MonoBehaviour
{
	private List<ParticleSystem> childParticleSystems;

	private List<ParticleSystem> ChildParticleSystems {
		get {
			if (childParticleSystems == null)
				childParticleSystems = new List<ParticleSystem> (GetComponentsInChildren<ParticleSystem> (true));
			return childParticleSystems;
		}
	}

	public UnityEvent onPlay;
	public UnityEvent onStop;

	public void PlayAfterDelay (float delay)
	{
		Invoke ("Play", delay);
	}

	public void Play ()
	{
		ChildParticleSystems.ForEach (p => p.Play ());
		onPlay.Invoke ();
	}

	public void StopAfterDelay (float delay)
	{
		Invoke ("Stop", delay);
	}

	public void Stop ()
	{
		ChildParticleSystems.ForEach (p => p.Stop ());
		onStop.Invoke ();
	}
}
