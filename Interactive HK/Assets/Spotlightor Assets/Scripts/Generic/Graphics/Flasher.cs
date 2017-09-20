using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flasher : MonoBehaviour
{
	public Transform target;
	public float visibleTime = 0.1f;
	public float invisibleTime = 0.03f;
	private List<Renderer> flashRenderers;

	private bool FlashRenderersEnabled {
		set { flashRenderers.ForEach (rd => rd.enabled = value);}
	}

	void Start ()
	{
		flashRenderers = new List<Renderer> ();

		Renderer[] childRenderers = target.GetComponentsInChildren<Renderer> ();
		foreach (Renderer rd in childRenderers) {
			if (!(rd is ParticleSystemRenderer))
				flashRenderers.Add (rd);
		}
	}

	public void Flash ()
	{
		StartCoroutine ("FlashLoop");
	}

	private IEnumerator FlashLoop ()
	{
		while (true) {
			FlashRenderersEnabled = false;

			yield return new WaitForSeconds (invisibleTime);

			FlashRenderersEnabled = true;

			yield return new WaitForSeconds (visibleTime);
		}
	}

	public void Stop ()
	{
		StopCoroutine ("FlashLoop");
		FlashRenderersEnabled = true;
	}
}
