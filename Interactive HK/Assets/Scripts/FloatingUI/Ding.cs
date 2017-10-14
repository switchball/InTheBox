using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]
public class Ding : MonoBehaviour {
    public Canvas canvas;
    public GameObject blackScreen;
    public Text text;

    public float waitTime;
    public string textShown;
    public bool hideRenderWhenPlay = true;

    private AudioSource audioSource;
    private bool flag;

    [ContextMenu("Init")]
    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("OverlayCanvas").GetComponent<Canvas>();
        blackScreen = canvas.transform.Find("BlackScreen").gameObject;
        text = blackScreen.transform.Find("Text").GetComponent<Text>();

        if (waitTime == 0)
            waitTime = 3;
        if (text && textShown == "")
            textShown = text.text.Replace("\n", "|");
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        flag = false;
        var ren = gameObject.GetComponent<Renderer>();
        if (hideRenderWhenPlay)
        {
            if (ren != null)
                ren.enabled = false;
        }
    }

    private void OnEnable()
    {
        flag = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!flag)
            {
                StartCoroutine(DingCoroutine());
                flag = true;
            }
        }
    }

    IEnumerator DingCoroutine()
    {
        text.text = textShown.Replace("|", "\n");
        blackScreen.SetActive(true);
        LockMove();
        audioSource.Play();
        yield return new WaitForSeconds(waitTime);
        blackScreen.SetActive(false);
        UnlockMove();
    }

    private void LockMove()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        var control = player.GetComponent<Controller>();
        var looker = player.GetComponent<MouseLooker>();
        // Disable Mouse Move
        if (looker)
            looker.DisableMove();

        // Disable WASD Move
        if (control)
            control.enabled = false;
    }

    private void UnlockMove()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        var control = player.GetComponent<Controller>();
        var looker = player.GetComponent<MouseLooker>();
        // Enable Mouse Move
        if (looker)
            looker.EnableMove();

        // Enable WASD Move
        if (control)
            control.enabled = true;
    }
}
