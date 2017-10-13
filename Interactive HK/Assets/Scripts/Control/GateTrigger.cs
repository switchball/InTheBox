using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour {

    public ReadingIt[] readings;
    public TransitionManager transitionManager;

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
                transitionManager.TransitionIn();
                mActivated = true;
            }

    }
}
