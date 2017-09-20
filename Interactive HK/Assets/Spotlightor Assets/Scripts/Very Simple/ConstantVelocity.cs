using UnityEngine;
using System.Collections;

public class ConstantVelocity : MonoBehaviour
{
	public Vector3 velocity = Vector3.forward;
	public bool isLocal = true;
	private Rigidbody myRigidbody;

	private Vector3 WorldVelocity {
		get { return isLocal ? transform.TransformVector (velocity) : velocity; }
	}

	void Start ()
	{
		myRigidbody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate ()
	{
		if (myRigidbody != null)
			myRigidbody.MovePosition (myRigidbody.position + WorldVelocity * Time.fixedDeltaTime);
	}

	void Update ()
	{
		if (myRigidbody == null)
			transform.position += WorldVelocity * Time.deltaTime;
	}
}
