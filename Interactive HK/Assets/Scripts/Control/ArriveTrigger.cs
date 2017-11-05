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

    public UnityEvent onFirstPass;

    private bool bPassed;

    public bool 是否强制移动视角 = false;

    [Header("仅当强制移动视角勾选时生效：")]
    public float 视角移动速度 = 1.5f;
    public float 锁定操作时间 = 2.0f;
    public Transform lookPosObj;


    MouseLooker locker;

    public void Awake()
    {
        locker = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLooker>();
        if (locker == null)
        {
            Debug.LogError("Player should contain MouseLooker script!");
        }
        if (lookPosObj == null)
        {
            if (transitionManager == null)
            {
                Debug.LogError("can not determine the look pos obj, set it please.");
            }
            lookPosObj = transitionManager.gameObject.transform;
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
        if (!IsInViewpoint() && 是否强制移动视角)
            TurnCamera();

        if (onArrive != null)
            onArrive.Invoke();
        if (!bPassed)
        {
            bPassed = true;
            // Invoke FirstPass Event
            if (onFirstPass != null)
                onFirstPass.Invoke();
        }
    }

    private void Update()
    {

    }

    private bool IsInViewpoint()
    {
        if (!Camera.main)
            return false;
        Vector3 pos = Camera.main.WorldToViewportPoint(lookPosObj.position);
        return 0.2f <= pos.x && pos.x <= 0.8f
            && 0.2f <= pos.y && pos.y <= 0.8f
            && 1.0f <= pos.z;
    }

    private void TurnCamera()
    {
        locker.LookAt(lookPosObj.position, 视角移动速度, 锁定操作时间);
    }
}
