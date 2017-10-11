using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class ReadingIt : MonoBehaviour {

    public Canvas canvas;
    public Image image;

    public float lastTimeAtLeast = 2.0f;

    private SpriteRenderer render;

    private GameObject player;
    private Controller control;
    private MouseLooker looker;

    private bool isReading = false;
    private float mTimeLeft;

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("OverlayCanvas").GetComponent<Canvas>();
        image = canvas.transform.Find("Panel/Image").GetComponent<Image>();
        Debug.LogWarning(image);
    }

    void Start()
    {
        render = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        control = player.GetComponent<Controller>();
        looker = player.GetComponent<MouseLooker>();
    }
	
	// Update is called once per frame
	void Update () {
		if (isReading)
        {
            mTimeLeft -= Time.deltaTime;
            if (mTimeLeft < 0)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    FinishReading();
                }
            }
        }
	}

    public void StartReading()
    {
        Debug.LogWarning("StartReading");
        // Disable Mouse Move
        if (looker)
            looker.DisableMove();

        // Disable WASD Move
        if (control)
            control.enabled = false;

        // Replace Image
        image.sprite = render.sprite;
        image.enabled = true;

        isReading = true;
        mTimeLeft = lastTimeAtLeast;
    }

    IEnumerator DelayedReading(float sec, bool s)
    {
        yield return new WaitForSeconds(sec);
        isReading = s;
    }

    public void FinishReading()
    {
        Debug.LogWarning("EndReading");

        // Enable Mouse Move
        if (looker)
            looker.EnableMove();

        // Enable WASD Move
        if (control)
            control.enabled = true;

        // Disable Image
        image.enabled = false;

        StartCoroutine(DelayedReading(0.2f, false));
        //isReading = false;
    }

    public bool GetReadingStatus()
    {
        return isReading;
    }
}
