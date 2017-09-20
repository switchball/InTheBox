using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDestructParticleSystem : MonoBehaviour
{
	public float delay = 1;
	// Use this for initialization
	IEnumerator Start ()
	{
		yield return new WaitForSeconds(delay);
		GetComponent<ParticleSystem>().Stop ();
		yield return new WaitForSeconds(GetComponent<ParticleSystem>().startLifetime);
		Destroy (gameObject);
	}
}
