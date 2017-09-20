using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContactAndSceneJump : MonoBehaviour {

    public int sceneBuildIndex = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("Goto Scene #" + sceneBuildIndex);
        SceneManager.LoadScene(sceneBuildIndex);
    }
}
