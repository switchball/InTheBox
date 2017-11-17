using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 相应鼠标的点击
public class RespondToAction : MonoBehaviour {
    [Tooltip("响应操作时玩家距离物体的最大范围")]
    public float RespondDistance;

    public TransitionManager transitionManager;

    public ReadingIt whatToRead;

    public bool 点击一次后消失 = true;
    public UnityEvent onClickIfNoReading;

    private GameObject player;
    private Animator anim;
    private bool mInView;

    private ResizeByDistance resizeCompoment;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        resizeCompoment = GetComponent<ResizeByDistance>();
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
            float a = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (a <= 0)
                a = 1;
            else if (a >= 1)
                a = 2;
            else
                a = 1 * a + 1;
            resizeCompoment.animScaleMultiplier = a;
            if (anim.GetBool("Clicked"))
            {
                resizeCompoment.animScaleMultiplier = 2;
                if (!mInView)
                {
                    anim.SetBool("Clicked", false);
                    anim.SetFloat("PlaySpeed", -1.0f);
                    anim.Play("CubeShow", 0, 1);
                }
            }
            else
                if (mInView)
                {
                    // Open Box 
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("CubeShow"))
                    {
                        anim.SetFloat("PlaySpeed", 1.0f);
                        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0)
                            anim.Play("CubeShow", -1, 0); // start from normalizedTime = 0
                    }
                    else
                    {
                        anim.Play("CubeShow");
                    }
                }
                else
                {
                    // Close Box
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("CubeShow"))
                    {
                        anim.SetFloat("PlaySpeed", -1.0f);
                        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                            anim.Play("CubeShow", -1, 1); // start from normalizedTime = 1
                    }
                    else
                    {
                        anim.Play("CubeShow");
                    }
                }
        }

        // 如果点击了鼠标，则触发transition动画
        if (mInView && Input.GetMouseButtonDown(0))
        {
            anim.SetBool("Clicked", true);
            anim.Play("CubeOpen");

            if (whatToRead) {
                if (whatToRead.GetReadingStatus() == false)
                    whatToRead.StartReading();
                //Debug.Log("点击了鼠标，触发transition动画: " + transitionManager);
            } else {
                onClickIfNoReading.Invoke();
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
        var a = gameObject.GetComponent<ColorTransitionManager>();
        if (a != null)
        {
            a.TransitionOut();
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
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
