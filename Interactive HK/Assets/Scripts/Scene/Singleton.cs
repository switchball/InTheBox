using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour {
    public string tagName;
	// Use this for initialization
	void Start () {
        tagName = gameObject.tag;
        int size = GameObject.FindGameObjectsWithTag(tagName).Length;
        Debug.LogError("size=" + size);
        if (size >= 2)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
