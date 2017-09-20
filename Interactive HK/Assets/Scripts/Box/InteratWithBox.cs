using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteratWithBox : MonoBehaviour {

    public float range = 5;

    public int defaultRotateSpeed = 0;
    public int hoverRotateSpeed = 30;
    RotateAround lastRotateScript;
    int clickableMask;
    //Ray shootRay;
    //RaycastHit shootHit;
    LineRenderer gunLine;

    ColorTransitionManager colorTranManager;
    bool lastFlag = false; // status of looking

    /* SR
    public ColorScreenTransition cst;
    public GameObject text;
    public GameObject title;
    */

    // Use this for initialization
    void Start () {
        clickableMask = LayerMask.GetMask("Button Layer");
        GameObject go = GameObject.Find("ButtonBoxSpecial");
        if (go != null)
            colorTranManager = go.GetComponent<ColorTransitionManager>();
        lastFlag = false;
    }

    // Update is called once per frame
    void Update () {
        bool isClick = Input.GetButtonDown("Fire1");
        bool flag = Check(isClick);
        if (!flag)
        {
            if (lastRotateScript != null) {
                lastRotateScript.speed = defaultRotateSpeed;
                lastRotateScript = null;
            }
        }
        if (flag != lastFlag)
        {
            if (flag && !lastFlag)
            {
                SwitchOn();
            } else {
                SwitchOff();
            }
            lastFlag = flag;
        }
        /*
        if (Input.GetButtonDown("Fire1"))
        {
            cst.color = Color.red;
            cst.TransitionIn(false);
            Debug.Log(cst.State);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            cst.TransitionOut();
            //GlobalBlackScreenTransition.Instance.TransitionOut(false);
            Debug.Log(cst.State);
        }
        if (Input.GetButtonDown("Fire3"))
        {

            WidgetTransitionManager wtm = text.GetComponent<WidgetTransitionManager>();
            wtm.TransitionIn();
            //TypeWriterTextDisplayer twtd = text.GetComponent<TypeWriterTextDisplayer>();
            //twtd.TypewriteText("这个街道限制着城市，  城市也限制着街道");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            WidgetTransitionManager wtm = text.GetComponent<WidgetTransitionManager>();
            wtm.TransitionOut();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            TransitionCommand command = title.GetComponent<TransitionCommand>();
            command.Execute();
        }
        */
    }

    private void SwitchOn()
    {
        if (colorTranManager != null)
        {
        Debug.Log("SwitchOn");
            colorTranManager.TransitionIn();

        }
    }
    private void SwitchOff()
    {
        Debug.Log("SwitchOff");
        if (colorTranManager != null)
            colorTranManager.TransitionOut();
    }

    private void OnGUI()
    {
        // # TODO
        // GUI.Label(new Rect(0, 0, 100, 50), "预览版本") ;

        //显示文字

    }

    private bool Check(bool isClick)
    {   
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit shootHit;

        if (Physics.Raycast(camRay, out shootHit, range, clickableMask)) {
            Collider coll = shootHit.collider;
            if (coll != null)
            {
                //Debug.LogWarning("collider is NOT null: " + coll);

                if (coll.CompareTag("TitleButton"))
                {
                    RotateAround x = coll.GetComponent<RotateAround>();
                    if (x != null)
                    {
                        lastRotateScript = x;
                        x.speed = hoverRotateSpeed;
                    }
                }

                if (isClick)
                {
                    StateTrigger trigger = coll.GetComponent<StateTrigger>();
                    if (trigger != null)
                    {
                        trigger.OuterChangeState(true);
                    }
                    else if (coll.CompareTag("ChoiceButton"))
                    {
                        string name = coll.name;
                        int which = -1;
                        if (name.Contains("AnswerA"))
                            which = 0;
                        else if (name.Contains("AnswerB"))
                            which = 1;
                        else if (name.Contains("AnswerC"))
                            which = 2;
                        else if (name.Contains("AnswerD"))
                            which = 3;
                        GameObject.Find("TempForListen").GetComponent<ListenMain>().ClickButtonAnswer(which);
                    }
                
                }
                else
                {

                }
            }
            return true;
        }
        else
        {
            //Debug.LogWarning("collider is null");
            return false;
        } // end of Raycast
    }
}
