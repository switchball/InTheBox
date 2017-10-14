﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 相应鼠标的点击
public class RespondToAction : MonoBehaviour {
    [Tooltip("响应操作时玩家距离物体的最大范围")]
    public float RespondDistance;

    public TransitionManager transitionManager;

    public ReadingIt whatToRead;

    public bool 点击一次后消失 = true;

    private GameObject player;
    private Animator anim;
    private bool mInView;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        if (RespondDistance == 0)
        {
            Debug.LogWarning("需要设置响应操作时玩家距离物体的最大范围，自动重置为5");
            RespondDistance = 5;
        }
        if (whatToRead == null)
            Debug.LogError("动画UI没有设置Reading的对象");
    }

    // Update is called once per frame
    void Update () {
        // 测距
        Vector3 v = player.transform.position - transform.position;
        float d = v.magnitude;
        if (d > RespondDistance)
        {
            mInView = false;
        } else
        {
            mInView = IsInViewpoint();
        }
        // TODO 如果在视野内，则播放动画
        if (anim != null)
        {
            if (mInView)
            {
                anim.SetBool("Reverse", false);
                //Debug.LogWarning("in view");
                anim.StopPlayback();
                anim.Play("CubeOpen0");
            }
            else
            {
                anim.SetBool("Reverse", true);
                //Debug.LogWarning("not in view");
                anim.StopPlayback();
                anim.Play("CubeClose0");
            }
        }

        // 如果点击了鼠标，则触发transition动画
        if (mInView && Input.GetMouseButtonDown(0))
        {
            if (whatToRead) {
                if (whatToRead.GetReadingStatus() == false)
                    whatToRead.StartReading();
                //Debug.Log("点击了鼠标，触发transition动画: " + transitionManager);

            } else {
            }
            if (点击一次后消失)
                StartCoroutine(LateDisable());
        }

    }

    public void StartTransition()
    {
        if (transitionManager)
            transitionManager.TransitionIn();
    }

    IEnumerator LateDisable()
    {

        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        //Do Function here...
    }

    private bool IsInViewpoint()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        return 0.1f <= pos.x && pos.x <= 0.9f
            && 0.1f <= pos.y && pos.y <= 0.9f
            && 0.1f <= pos.z;
    }
}
