using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneJump : MonoBehaviour {
    public UnityEvent onClick;
    private bool flag;

    private void Start()
    {

    }
    
    void Update () {


    }

    public void GoToNextScene (int scene) {
        SceneManager.LoadScene(scene);

    }

    IEnumerator L(int scene)
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(scene);
    }

    public void GoToNextSceneWithDelayed (int scene)
    {
        StartCoroutine(L(scene));
    }
}
