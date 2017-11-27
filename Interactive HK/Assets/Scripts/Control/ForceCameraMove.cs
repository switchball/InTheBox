using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCameraMove : MonoBehaviour {
    public float 视角移动速度 = 5f;
    public float 锁定操作时间 = 2.5f;
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
            Debug.LogWarning("can not determine the look pos obj, set it please.");
        }
    }

    private bool IsInViewpoint(Transform obj)
    {
        if (!Camera.main)
            return false;
        if (obj == null)
            return true;
        Vector3 pos = Camera.main.WorldToViewportPoint(obj.position);
        return 0.2f <= pos.x && pos.x <= 0.8f
            && 0.2f <= pos.y && pos.y <= 0.8f
            && 1.0f <= pos.z;
    }

    public void ForceTurnCamera()
    {
        if (IsInViewpoint(lookPosObj))
            return;
        locker.LookAt(lookPosObj.position, 视角移动速度, 锁定操作时间);
    }
}
