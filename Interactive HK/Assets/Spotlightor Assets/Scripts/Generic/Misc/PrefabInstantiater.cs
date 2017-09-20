using UnityEngine;
using System.Collections;

public class PrefabInstantiater : MonoBehaviour
{
	public GameObject prefab;
	public bool addAsChild = false;
	public bool instantiateOnAwake = false;

	void OnEnable ()
	{
		if (instantiateOnAwake)
			Instantiate ();
	}


	public void Instantiate ()
	{
		GameObject instance = GameObject.Instantiate (prefab, transform.position, transform.rotation) as GameObject;

		if (addAsChild)
			instance.transform.SetParent (transform, true);
	}
}
