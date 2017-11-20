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

        if (Input.GetKeyDown(KeyCode.F12))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    public void SpecialResetPlayerPosDelayed(float t)
    {
        StartCoroutine(D(t));
    }

    IEnumerator D(float t)
    {
        yield return new WaitForSeconds(t);
        Vector3 p = new Vector3(18.5f, -0.5f, 5.0f);
        GameObject.FindGameObjectWithTag("Player").transform.position = p;
    }
    
}
