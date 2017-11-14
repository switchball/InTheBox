using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerByDistance : MonoBehaviour {
    [Tooltip("始终调整为距离物体多近时的视角大小")]
    public float visibleDistance;

    private GameObject player;
    private MouseLooker looker;

    public UnityEvent onFirstSight;
    private bool firstSightTriggered = false;
    private float distance;


    void Start()
    {
        if (visibleDistance == 0)
        {
            Debug.LogWarning("需要设置默认视角距离，自动重置为10");
            visibleDistance = 10;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        looker = player.GetComponent<MouseLooker>();
    }

    void Update()
    {
        Vector3 v = player.transform.position - transform.position;
        distance = v.magnitude;
        if (looker)
        {
            Quaternion q1 = looker.GetRotation();
            Quaternion q2 = Quaternion.LookRotation(-v);
            float angle = Quaternion.Angle(q1, q2);
            var d = distance * Mathf.Cos(Mathf.Deg2Rad * angle);
            //Debug.Log("Angle: " + angle + ", distace: " + distance + ", d=" + d);
            distance = d;
        }

        // distance to event
        if (!firstSightTriggered)
        {
            if (distance < visibleDistance)
            {
                if (IsInViewpoint())
                {
                    onFirstSight.Invoke();
                    firstSightTriggered = true;

                }
            }
        }
    }

    private bool IsInViewpoint()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        return 0.1f <= pos.x && pos.x <= 0.9f
            && 0.1f <= pos.y && pos.y <= 0.9f
            && 0.1f <= pos.z;
    }
}
