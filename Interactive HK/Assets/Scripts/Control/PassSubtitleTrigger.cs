using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class PassSubtitleTrigger : MonoBehaviour {

    public TransitionManager tm;

	public void OnTriggerEnter(Collider coll)
    {
        tm.TransitionIn();
    }

    public void OnTriggerExit(Collider coll)
    {
        tm.TransitionOut();
    }
}
