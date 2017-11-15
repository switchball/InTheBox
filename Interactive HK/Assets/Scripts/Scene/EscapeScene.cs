using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeScene : MonoBehaviour {

    private ReadingIt[] readinglist;
    private float timestamp;

    private bool isPressed = false;

	// Use this for initialization
	void Start () {
        readinglist = FindObjectsOfType<ReadingIt>();
	}
	
	// Update is called once per frame
	void Update () {
        timestamp += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            timestamp = 0;
            isPressed = true;

        }
        if (isPressed)
        {
            if (timestamp >= 1.5f)
                SceneManager.LoadScene(0);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isPressed = false;
        }

    }
    
}
