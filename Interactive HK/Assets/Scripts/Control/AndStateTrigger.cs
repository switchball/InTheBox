using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndStateTrigger : StateTrigger
{
    public List<StateTrigger> childs;

	// Use this for initialization
	void Start () {
        status = true;
        foreach (var child in childs) {
            child.SetAndParent(this);
            if (child.GetStatus() == false) {
                status = false;
            }
        }
        SwitchState(status);
	}
	
	public override void OnReceiveChildStateChange(StateTrigger child, bool state) {
        base.OnReceiveChildStateChange(child, state);
        Debug.Log(child + ":" + state);
        bool new_status = true;
        foreach (var c in childs)
        {
            if (c.GetStatus() == false)
            {
                new_status = false;
                break;
            }
        }
        if (new_status != status)
            OuterChangeState(new_status);
    }
}
