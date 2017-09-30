using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public float distanceThreshold = 5.0f;
    public float changeSpeed = 5.0f;
    public Renderer render;
    private GameObject player;
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void FixedUpdate () {
        Vector3 distance = player.transform.position - gameObject.transform.position;
        Color tempColor = gameObject.GetComponent<Renderer>().material.GetColor("_TintColor");
        if (distance.x * distance.x + distance.y * distance.y + distance.z * distance.z < distanceThreshold * distanceThreshold) {
            render.enabled = true;
            if (tempColor.a < 1) {
                tempColor.a += changeSpeed * 1 / 255.0f;
                gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", tempColor);
            }
        } else {
            if(tempColor.a > 0) {
                tempColor.a -= changeSpeed * 1 / 255.0f;
                gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", tempColor);
            } else {
                render.enabled = false;
            }
        }
    }
}