using UnityEngine;
using System.Collections;

public class S2_zxc_Trigger : MonoBehaviour {
	GameObject player;
	Plane2DMover moverScript;
	Animator anim;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		moverScript = player.GetComponent<Plane2DMover> ();
		anim = player.GetComponent<Animator> ();

		//anim.enabled = false;
		player.transform.position = Vector3.left * 8;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Player")) {
			Debug.Log ("zxc trigger");
			moverScript.enabled = false;
			anim.enabled = true;
			player.transform.position = gameObject.transform.position;
		}
	}
}
