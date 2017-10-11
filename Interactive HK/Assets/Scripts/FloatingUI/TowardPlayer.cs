using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowardPlayer : MonoBehaviour {

    private GameObject player;
    private MouseLooker locker;
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        locker = player.GetComponent<MouseLooker>();
    }
	
	void Update () {
        //Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        if (locker != null && locker.enabled)
        {
            Quaternion rotation = locker.GetRotation();
            gameObject.transform.rotation = rotation;
        }
        //gameObject.transform.eulerAngles = gameObject.transform.eulerAngles + new Vector3(0, 180, 0);
    }
}
