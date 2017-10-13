using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ArriveTrigger : MonoBehaviour {

    public TransitionManager transitionManager;
    public bool hideRenderWhenPlay = true;
    public UnityEvent onArrive;
    

    MouseLooker locker;

    public void Awake()
    {
        locker = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLooker>();
        if (locker == null)
        {
            Debug.LogError("Player should contain MouseLooker script!");
        }
    }

    private void Start()
    {
        var ren = gameObject.GetComponent<Renderer>();
        if (hideRenderWhenPlay)
        {
            if (ren != null)
                ren.enabled = false;
        }
        //RectTransformUtility.ScreenPointToLocalPointInRectangle()
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (transitionManager)
            transitionManager.TransitionIn();
        // 如果目标不在视野内，且强制移动视角开启
        if (onArrive != null)
            onArrive.Invoke();
    }

    private void Update()
    {

    }
    
    
}
