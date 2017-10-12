using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PassSubtitleTrigger : MonoBehaviour {

    public TransitionManager transitionManager;
    public bool hideRenderWhenPlay = true;

    public bool 是否强制移动视角 = true;

    [Header("仅当强制移动视角勾选时生效：")]
    public float 视角移动速度 = 1.5f;
    public float 锁定操作时间 = 2.0f;

    public UnityEvent onFirstPass;

    private bool bPassed;

    MouseLooker locker;
    Vector3 posSubtitle;

    public void Awake()
    {
        posSubtitle = transitionManager.gameObject.transform.position;
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
        bPassed = false;
    }

    public void OnTriggerEnter(Collider coll)
    {
        transitionManager.TransitionIn();
        // 如果目标不在视野内，且强制移动视角开启
        if (!IsInViewpoint() && 是否强制移动视角)
            TurnCamera();
    }

    public void OnTriggerExit(Collider coll)
    {
        transitionManager.TransitionOut();
        if (!bPassed)
        {
            bPassed = true;
            // Invoke FirstPass Event
            onFirstPass.Invoke();
        }
    }

    private void Update()
    {
        bool b = IsInViewpoint();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Debug.Log("?:" + Camera.main.WorldToViewportPoint(posSubtitle) + b);
            TurnCamera();
        }
    }

    private bool IsInViewpoint()
    {
        if (!Camera.main)
            return false;
        Vector3 pos = Camera.main.WorldToViewportPoint(posSubtitle);
        return 0.2f <= pos.x && pos.x <= 0.8f
            && 0.2f <= pos.y && pos.y <= 0.8f
            && 1.0f <= pos.z;
    }

    private void TurnCamera()
    {
        locker.LookAt(posSubtitle, 1.5f, 2.0f);
    }
    
}
