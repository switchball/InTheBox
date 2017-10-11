using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 根据与Player距离的远近，调整scale
public class ResizeByDistance : MonoBehaviour {

    [Tooltip("始终调整为距离物体多近时的视角大小")]
    public float standardDistance;
    [Tooltip("多近距离范围内可以看见这个物体")]
    public Vector2 visibleDistance;
    private Vector3 mScale;
    private GameObject player;
    private MouseLooker looker;

    private float distance;

    [ColorUsage(true, true, 0, 8, 0.125f, 3)]
    public Color colorIn = Color.white;
    [ColorUsage(true, true, 0, 8, 0.125f, 3)]
    public Color colorOut = new Color(1, 1, 1, 0);

    private ColorDisplayer colorDisplayer;

    public ColorDisplayer ColorDisplayer
    {
        get
        {
            if (colorDisplayer == null)
            {
                colorDisplayer = GetComponent<ColorDisplayer>();
                if (colorDisplayer == null)
                    colorDisplayer = gameObject.AddComponent<ColorDisplayer>();
            }
            return colorDisplayer;
        }
    }

    void Start () {
        mScale = transform.localScale;
        if (standardDistance == 0)
        {
            Debug.LogWarning("需要设置默认视角距离，自动重置为10");
            standardDistance = 10;
        }
        if (visibleDistance == null)
        {
            Debug.LogWarning("需要设置默认视角距离，自动重置为standardDistance");
            visibleDistance = new Vector2(standardDistance/2, standardDistance);
        }
        player = GameObject.FindGameObjectWithTag("Player");
        looker = player.GetComponent<MouseLooker>();
    }

    // Update is called once per frame
    void Update () {
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
        float ratio = distance / standardDistance;
        transform.localScale = mScale * ratio;
        float progress = Mathf.InverseLerp(visibleDistance.x, visibleDistance.y, distance);
        ColorDisplayer.Display(Color.Lerp(colorIn, colorOut, progress));
    }
}
