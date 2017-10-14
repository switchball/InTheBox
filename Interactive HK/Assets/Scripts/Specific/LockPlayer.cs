using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPlayer : MonoBehaviour {
    private MouseLooker looker;
    private Controller control;

	// Use this for initialization
	void Start () {
        looker = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLooker>();
        control= GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Lock()
    {
        looker.enabled = false;
        control.enabled = false;
    }

    public void Unlock(float sec)
    {
        StartCoroutine(U(sec));
    }

    IEnumerator U(float sec)
    {
        yield return new WaitForSeconds(sec);

        looker.enabled = true;
        control.enabled = true;
    }
}
