using UnityEngine;
using System.Collections;

public class S2_Point_Routine : MonoBehaviour {
	public GameObject objZ;
	public GameObject objX;
	public GameObject objC;
	public GameObject objV;


	Animator anim;

	bool actZ;
	bool actX;
	bool actC;
	bool actV;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		Reset ();
	}

	void Reset () {
		actZ = true;
		actX = true;
		actC = true;
		actV = true;
		objZ.SetActive (actZ);
		objX.SetActive (actX);
		objC.SetActive (actC);
		objV.SetActive (actV);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			actZ = !actZ;
			objZ.SetActive (actZ);
			anim.SetBool ("StartBool", true);

		} 
		if (Input.GetKeyDown (KeyCode.X)) {
			actX = !actX;
			objX.SetActive (actX);
		} 
		if (Input.GetKeyDown (KeyCode.C)) {
			actC = !actC;
			objC.SetActive (actC);
		} 
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Blocked")) {
			Debug.Log ("coll");
			//anim.Stop ();
			anim.SetBool("StartBool", false);
			anim.SetTrigger ("Replay");
			//anim.ResetTrigger ("End");
			//anim.Play ("S2_PointAnimation", -1, 0f);
			Reset();
		}
	}

	public void OpenDoorV () {
		actV = false;
		objV.SetActive (actV);
	}
}
