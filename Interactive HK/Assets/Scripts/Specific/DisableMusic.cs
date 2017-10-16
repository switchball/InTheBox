using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMusic : MonoBehaviour {
    public GameObject obj;


	// Use this for initialization
	void Start () {
        StartCoroutine(A());
    }

    IEnumerator A()
    {
        yield return new WaitForSeconds(1.0f);
        obj = GameObject.FindGameObjectWithTag("SingleBGM");
    }

    public void DisableBGM()
    {
        obj.GetComponent<TransitionManager>().TransitionOut();
        StartCoroutine(DisableGO());
    }

    IEnumerator DisableGO()
    {
        yield return new WaitForSeconds(3.0f);
        obj.SetActive(false);
        Destroy(obj);
    }
}
