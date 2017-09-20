using UnityEngine;
using System.Collections;

public class PhysicalExploder : MonoBehaviour
{
	public Rigidbody[] allDebris;
	public float explosionForce = 10;
	public Vector3 randomForce = Vector3.zero;
	public Vector3 randomTorque = Vector3.zero;
	public float explosionRadius = 10;
	public bool explodeOnAwake = false;

	void Start ()
	{
		if (explodeOnAwake)
			Explode ();
	}

	public void Explode ()
	{
		ExplodeAt (transform.position);
	}

	public void ExplodeAt (Transform target)
	{
		ExplodeAt (target.position);
	}

	public void ExplodeAt (Vector3 explosionPosition)
	{
		foreach (Rigidbody partRigidbody in allDebris) {
			partRigidbody.useGravity = true;
			partRigidbody.isKinematic = false;

			Collider partCollider = partRigidbody.GetComponent<Collider> ();
			if (partCollider != null)
				partCollider.enabled = true;

			partRigidbody.AddExplosionForce (explosionForce, explosionPosition, explosionRadius);

			if (randomForce != Vector3.zero)
				partRigidbody.AddForce (new Vector3 (Random.Range (-1f, 1f) * randomForce.x, Random.Range (-1f, 1f) * randomForce.y, Random.Range (-1f, 1f) * randomForce.z), ForceMode.Impulse);
			if (randomTorque != Vector3.zero)
				partRigidbody.AddTorque (new Vector3 (Random.Range (-1f, 1f) * randomTorque.x, Random.Range (-1f, 1f) * randomTorque.y, Random.Range (-1f, 1f) * randomTorque.z), ForceMode.Impulse);
		}
	}
	
	void Reset ()
	{
		if (allDebris == null || allDebris.Length == 0)
			allDebris = GetComponentsInChildren<Rigidbody> (true);
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, explosionRadius);
	}
}
