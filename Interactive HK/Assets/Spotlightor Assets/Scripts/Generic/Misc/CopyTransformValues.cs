using UnityEngine;
using System.Collections;

public class CopyTransformValues : MonoBehaviour
{
	public Transform target;
	public bool copyPosition = true;
	public bool copyRotation = true;

	void LateUpdate ()
	{
		if (copyPosition)
			transform.position = target.position;
		if (copyRotation)
			transform.rotation = target.rotation;
	}
}
