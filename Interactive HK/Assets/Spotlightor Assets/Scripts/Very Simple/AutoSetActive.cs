using UnityEngine;
using System.Collections;

public class AutoSetActive : MonoBehaviour
{
	public bool active = false;
	public float delay = 0;
	public GameObject target;

	private GameObject ValidTarget {
		get { return target != null ? target : this.gameObject; }
	}

	void Awake ()
	{
		if (delay <= 0)
			ValidTarget.SetActive (active);
	}

	void OnEnable ()
	{
		if (delay > 0)
			StartCoroutine ("DelayAndSetActive");
	}

	void OnDisable ()
	{
		StopCoroutine ("DelayAndSetActive");
	}

	private IEnumerator DelayAndSetActive ()
	{
		yield return new WaitForSeconds (delay);

		ValidTarget.SetActive (active);
	}
}
