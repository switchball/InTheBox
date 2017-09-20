using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S2_BlackDelay : MonoBehaviour {
	public List<GameObject> tempHidden;

	public float timeDelayed = 3;

	GameObject player;
	Light light;

	float acc = 0;
	bool active = false;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		light = gameObject.GetComponent<Light> ();
		active = true;
	}

	void Update () {
		if (active)
			acc += Time.deltaTime;

		if (acc > timeDelayed) {
			// enable player
			player.GetComponent<SpriteRenderer>().enabled = true;
			player.GetComponent<Plane2DMover> ().enabled = true;

			// enable light
			light.enabled = true;

			// enable temp hidden
			foreach (var s in tempHidden) {
				s.SetActive (true);
			}
		}
			
	}

}
