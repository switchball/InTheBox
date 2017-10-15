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
        anim.Play("CubeClose0");
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
        anim.Play("CubeOpen0");
    }

    private void OnMouseExit()
    {
        anim.Play("CubeClose0");

    }

    void Update () {


	}

	public void GoToNextScene (int scene) {
        SceneManager.LoadScene(scene);

    }
}
