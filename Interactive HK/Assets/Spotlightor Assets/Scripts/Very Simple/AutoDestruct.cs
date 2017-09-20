using UnityEngine;
using System.Collections;

public class AutoDestruct : MonoBehaviour
{
	public float delay = 3;

	void Awake ()
	{
		if (delay <= 0 && enabled)
			GameObject.Destroy (gameObject);
	}

	void Start ()
	{
		if (delay > 0)
			GameObject.Destroy (gameObject, delay);
		else
			GameObject.Destroy (gameObject);
	}
}

