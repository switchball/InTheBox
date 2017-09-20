using UnityEngine;
using System.Collections;

public class AutoEnable : MonoBehaviour
{
	public bool setEnabled = true;
	public float delay = 0;
	public Behaviour target;

	void Awake ()
	{
		if (delay <= 0 && target != null)
			target.enabled = setEnabled;
	}

	void OnEnable ()
	{
		if (delay > 0)
			StartCoroutine ("DelayAndSetEnabled");
	}

	void OnDisable ()
	{
		StopCoroutine ("DelayAndSetEnabled");
	}

	private IEnumerator DelayAndSetEnabled ()
	{
		yield return new WaitForSeconds (delay);

		if (target != null)
			target.enabled = setEnabled;
	}
}
