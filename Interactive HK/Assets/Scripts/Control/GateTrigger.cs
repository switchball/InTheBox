using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour {

    public ReadingIt[] readings;
    public TransitionManager transitionManager;
    public float delayTranTime;
    public bool TurnToTarget = false;

    private bool mActivated = false;

	// Use this for initialization
	void Start () {
        if (readings.Length == 0)
            Debug.LogError("No Readings script!");
	}
	
	// Update is called once per frame
	void Update () {
        if (readings.Length == 0)
            return;
        bool a = true;
		foreach (var read in readings)
        {
            a = a && read.HasRead();
        }
        if (a && !mActivated)
            if (transitionManager)
            {
                //transitionManager.TransitionIn();
                StartCoroutine(T());
                mActivated = true;
            }

    }

    IEnumerator T()
    {
        yield return new WaitForSeconds(delayTranTime);
        transitionManager.TransitionIn();
        TurnCamera();
    }


    private void TurnCamera()
    {
        var locker = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLooker>();
        if (locker)
            locker.LookAt(transitionManager.gameObject.transform.position, 1.5f, 2.0f);
    }
}
