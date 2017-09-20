using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S2_CameraLookInto : MonoBehaviour {

    public Camera cam;

    public float forceDelay = 3.0f;

    public bool stay = false;

    public bool force = false;

    public AudioTransitionManager audioTranManager;

    float accTime = 0;

	// Use this for initialization
	void Start () {
        cam.orthographicSize = 5;
    }
	
	// Update is called once per frame
	void Update () {
        if (force & stay) {
            accTime += Time.deltaTime;
            cam.orthographicSize = Mathf.Lerp(1.0f, 0.1f, accTime / forceDelay);
            if (accTime > forceDelay) {
                // Jump Scene
                Debug.Log("Goto Scene #0");
                SceneManager.LoadScene(0);
            }
            return;
        }
        if (stay)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 0.85f, Time.deltaTime / 2);
        } else
        {
            force = false;
            accTime = 0;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5, Time.deltaTime / 3);
        }
        if (cam.orthographicSize < 1) {
            force = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stay = true;
        if (audioTranManager != null)
            audioTranManager.TransitionOut();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        stay = false;
        if (audioTranManager != null)
            audioTranManager.TransitionIn();
        Debug.Log(collision);
    }
}
