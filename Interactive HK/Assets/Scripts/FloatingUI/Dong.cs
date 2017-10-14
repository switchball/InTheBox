using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class Dong : MonoBehaviour {
    public Canvas canvas;
    public Text text;

    public float waitTime;
    public string textShown;

    private bool flag;

    [ContextMenu("Init")]
    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("OverlayCanvas").GetComponent<Canvas>();
        text = blackScreen.transform.Find("CenterText").GetComponent<Text>();

        if (waitTime == 0)
            waitTime = 3;
        if (text && textShown == "")
            textShown = text.text.Replace("\n", "|");
    }


    void Start () {
        flag = false;
	}

    private void OnEnable()
    {
        flag = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!flag)
            {
                flag = true;
                text.GetComponent<TransitionManager>().TransitionIn();
                // add event
            }
        }
    }
}
