using UnityEngine;
using System.Collections;

#if !UNITY_5_4_OR_NEWER
[RequireComponent(typeof(ParticleEmitter))]
#endif
public class AutoDestructParticleVFX : MonoBehaviour
{
	public float delay = 1;

	#if UNITY_5_4_OR_NEWER
	void Start ()
	{
		this.Log ("ParticleEmitter is decrepit");
	}
	#else
	IEnumerator Start ()
	{
		yield return new WaitForSeconds (delay);
		GetComponent<ParticleEmitter> ().emit = false;
		yield return new WaitForSeconds (GetComponent<ParticleEmitter> ().maxEnergy);
		Destroy (gameObject);
	}
	#endif
}
