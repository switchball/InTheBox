using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowardPlayer : MonoBehaviour {

    private GameObject player;
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	void Update () {
        Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        gameObject.transform.rotation = rotation;
        gameObject.transform.eulerAngles = gameObject.transform.eulerAngles + new Vector3(90, 0, 0);
    }
}
