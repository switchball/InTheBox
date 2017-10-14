using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Dong : MonoBehaviour {
    public Canvas canvas;
    public Text text;

    public float waitTime;
    public string textShown;
    public bool hideRenderWhenPlay = true;

    public UnityEvent onTrigger;

    private bool flag;

    [ContextMenu("Init")]
    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("OverlayCanvas").GetComponent<Canvas>();
        text = canvas.transform.Find("CenterText").GetComponent<Text>();

        if (waitTime == 0)
            waitTime = 3;
        if (text && textShown == "")
            textShown = text.text.Replace("\n", "|");
    }


    void Start () {
        flag = false;
        var ren = gameObject.GetComponent<Renderer>();
        if (hideRenderWhenPlay)
        {
            if (ren != null)
                ren.enabled = false;
        }
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
                text.text = textShown.Replace("|", "\n");
                var tm = text.GetComponent<TransitionCommand>();
                if (tm)
                {
                    tm.inAndOutWaitTime = waitTime;
                    tm.Execute();
                }
                // add event
                onTrigger.Invoke();
            }
        }
    }
}
