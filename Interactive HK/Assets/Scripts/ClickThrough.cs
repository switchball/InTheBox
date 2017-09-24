using UnityEngine;
using System.Collections;

public class ClickThrough : MonoBehaviour {
	
	public float range;

	int clickableMask;
	Ray shootRay;
	RaycastHit shootHit;
	LineRenderer gunLine;

	void Awake () {
		clickableMask = LayerMask.GetMask ("Button Layer");
		clickableMask = LayerMask.GetMask ("UI");
		Debug.Log (clickableMask);
		gunLine = GetComponent <LineRenderer> ();
	}

	// Use this for initialization
	void Start () {
		//amm = GameObject.Find ("GameManager").GetComponent<AudioMatchManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown ("Fire1") )
		{
			// ... click 
			Click ();
		}
	}

	void Click () {
		// Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;

		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);
		gunLine.SetWidth (0.03f, 0.01f);

		// Perform the raycast against gameobjects on the shootable layer and if it hits something...
		if(Physics.Raycast (shootRay, out shootHit, range, clickableMask))
		{
			// Try and find an StateTrigger script on the gameobject hit.
			Collider coll = shootHit.collider;
			if (coll != null) {
                StateTrigger trigger = coll.GetComponent<StateTrigger>();
                if (trigger != null) {
                    trigger.OuterChangeState(true);
                } else if (coll.CompareTag ("AudioButton")) {
					
				} else if (coll.CompareTag ("PictureLabel")) {
					
				} else if (coll.CompareTag ("TextZone")) {
					
				} else if (coll.CompareTag ("ChoiceButton")) {
					string name = coll.name;
					int which = -1;
					if (name.Contains("AnswerA"))
						which = 0;
					else if (name.Contains("AnswerB"))
						which = 1;
					else if (name.Contains("AnswerC"))
						which = 2;
					else if (name.Contains("AnswerD"))
						which = 3;
					GameObject.Find ("TempForListen").GetComponent<ListenMain> ().ClickButtonAnswer (which);
				} else {
					Debug.LogWarning ("no tag #1");
				}
			}

			// Set the second position of the line renderer to the point the raycast hit.
			gunLine.SetPosition (1, shootHit.point);
		}
		// If the raycast didn't hit anything on the shootable layer...
		else
		{
			// nothing
			Debug.Log ("Nothing Hit");
			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
	}

	private void FadeIn (MeshRenderer render, float duration) {
		
	}

}
