using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSkybox : MonoBehaviour {

    public float speed = 15f;

    float curRot = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        curRot += speed * Time.deltaTime;
        curRot %= 360;
        RenderSettings.skybox.SetFloat("_Rotation", curRot);
    }
}
