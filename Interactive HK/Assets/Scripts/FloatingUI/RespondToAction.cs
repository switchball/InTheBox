using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 相应鼠标的点击
public class RespondToAction : MonoBehaviour {
    [Tooltip("响应操作时玩家距离物体的最大范围")]
    public float RespondDistance;

    public TransitionManager transitionManager;

    private GameObject player;
    private bool mInView;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        if (RespondDistance == 0)
        {
            Debug.LogWarning("需要设置响应操作时玩家距离物体的最大范围，自动重置为5");
            RespondDistance = 5;
        }
    }

    // Update is called once per frame
    void Update () {
        // 测距
        Vector3 v = player.transform.position - transform.position;
        float d = v.magnitude;
        if (d > RespondDistance)
        {
            mInView = false;
            return;
        } else
        {
            mInView = IsInViewpoint();
        }
        // TODO 如果在视野内，则播放动画

        // 如果点击了鼠标，则触发transition动画
        if (mInView && Input.GetMouseButtonDown(0))
        {
            Debug.Log("点击了鼠标，触发transition动画: " + transitionManager);
            transitionManager.TransitionIn();
            gameObject.SetActive(false);
        }

    }

    private bool IsInViewpoint()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        return 0.2f <= pos.x && pos.x <= 0.8f
            && 0.2f <= pos.y && pos.y <= 0.8f
            && 1.0f <= pos.z;
    }
}
