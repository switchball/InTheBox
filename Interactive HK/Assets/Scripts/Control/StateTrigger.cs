using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



// Version 1.0
// 用途：放在带trigger的碰撞盒上，
// 当触发时，可以隐藏一些object，或者显示一些object
// 例如，当要切换bgm从1到2时，
//  - 把 bgm1 拖入 objectsToHide
//  - 把 bgm2 拖入 objectsToShow
public class StateTrigger : MonoBehaviour {
    [Tooltip("Objects to show when trigger")]
    public List<GameObject> objectsToShow; // 当触发时要显示的物体
    [Tooltip("Objects to hide when trigger")]
    public List<GameObject> objectsToHide; // 当触发时要显示的物体

    [Tooltip("Command to execute when trigger")]
    public CoroutineCommandBehavior command;

    [Tooltip("If true, the mesh render will disapper\nIf false, it is debug mode (default)")]
    public bool hideRenderWhenPlay;

    //[Tooltip("If true, the trigger will active only once (default)\nIf false, it deactives when exit")]
    //public bool destroySelfAfterTrigger = true;

    [Tooltip("If true, the trigger will active forever (default)\nIf false, it deactives when exit")]
    public bool persistActiveState = true;

    Renderer ren;

    AndStateTrigger andTrigger;

    //[HideInInspector]
    [Tooltip("Initial status (debug use), default: false")]
    public bool status = false;

    public UnityEvent onTrigger;

    bool reverse = false;


    // Use this for initialization
    void Start () {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p == null)
            Debug.Log("NO PLAYER TAG!!!");

        ren = gameObject.GetComponent<Renderer>();
        if (hideRenderWhenPlay) {
            if (ren != null)
                ren.enabled = false;
        }
        SwitchState(status);
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider coll) {
        if (coll.CompareTag("Player")) {
            SwitchState(true);
            //status = true;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (persistActiveState)
            return;

        if (coll.CompareTag("Player"))
        {
            //status = false;
            SwitchState(false);
        }
    }

    public void OuterChangeState(bool s) {
        Debug.Log("State change from outside: " + s);
        SwitchState(s);

        // if not persist, change it back
        if (!persistActiveState && s)
            status = false;
    }

    protected void SwitchState(bool b)
    {
        if (reverse)
            b = !b;
        if (status == b)
            return;
        status = b;
        //if (status && b && persistActiveState)
        //    return; // do not trigger second time
        Debug.Log("Switch state =" + b);
        foreach (var obj in objectsToShow)
        {
            Text checkText = obj.GetComponent<Text>();
            if (checkText != null) {
                checkText.enabled = b;
            } else {
                obj.SetActive(b);
            }
        }
        foreach (var obj in objectsToHide)
        {
            Debug.LogWarning("obj = " + obj + " this = " + gameObject);
            Text checkText = obj.GetComponent<Text>();
            if (checkText != null)
            {
                checkText.enabled = !b;
            }
            else
            {
                obj.SetActive(!b);
            }
        }
        // if status is true then execute command
        if (b)
        {
            if (command != null)
                command.Execute();
            var x = gameObject.GetComponent<ScaleTransitionManager>();
            if (x != null && x.enabled)
                x.TransitionOut();

            onTrigger.Invoke();
        }

        //if (b && destroySelfAfterTrigger) {
        //    gameObject.SetActive(false);
        //}
        SendToParent(b);
    }

    // called when state changes
    void SendToParent(bool b)
    {
        if (andTrigger != null)
        {
            andTrigger.OnReceiveChildStateChange(this, b);
        }
    }

    public virtual void OnReceiveChildStateChange(StateTrigger child, bool state) {
        // Not implemented
    }

    public bool GetStatus() {
        if (reverse)
            return !status;
        else
            return status;
    }

    public void SetAndParent(AndStateTrigger a) {
        andTrigger = a;
    }
}
