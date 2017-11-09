using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneJump : MonoBehaviour {
    public UnityEvent onClick;
    private Animator anim;
    private bool flag;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("CubeOpen");
        anim.SetFloat("PlaySpeed", 0.0f);
    }

    private void OnMouseDown()
    {
        if (!flag)
        {
            onClick.Invoke();
            flag = true;
        }
    }

    private void OnMouseEnter()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("CubeOpen0"))
        {
            anim.SetFloat("PlaySpeed", 1.0f);
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0)
                anim.Play("CubeOpen0", -1, 0); // start from normalizedTime = 0
        } else
        {
            anim.Play("CubeOpen0");
        }
    }

    private void OnMouseExit()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("CubeOpen0"))
        {
            anim.SetFloat("PlaySpeed", -1.0f);
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                anim.Play("CubeOpen0", -1, 1); // start from normalizedTime = 1
        }
        else
        {
            anim.Play("CubeOpen0");
        }

    }

    void Update () {


    }

    public void GoToNextScene (int scene) {
        SceneManager.LoadScene(scene);

    }
}
