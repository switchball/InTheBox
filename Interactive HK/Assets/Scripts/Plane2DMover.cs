using UnityEngine;
using System.Collections;

// do not chongyong this script
public class Plane2DMover : MonoBehaviour {
	public float speedX = 1f;
	public float speedY = 1f;
	public Transform initPos;

	public bool rotated = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis ("Horizontal") * speedX;
		float v = Input.GetAxis ("Vertical")   * speedY;

		if (rotated)
			transform.Translate(new Vector3(0, v, -h));
		else
			transform.Translate(new Vector3(h, v, 0));
	}

	void reset () {
		gameObject.transform.position = initPos.position;
		gameObject.transform.eulerAngles = Vector3.zero;
	}

	void OnTriggerEnter(Collider coll) {
		Debug.Log ("aaa" + coll.gameObject.tag);

		if (coll.gameObject.tag == "Blocked") {
			reset ();
			Debug.Log ("bbb");	
		} //else if (coll.gameObject.tag == "Blocked2") {
		//	coll.enabled = false;
		//}
	}

		
}
