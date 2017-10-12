using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class ReadingIt : MonoBehaviour {
    [Header("使用方法：拖到带有图片的组件上")]
    [Header("然后右击选择->Init，可自动完成部分设置")]
    public Canvas canvas;
    public Image image;
    public Text text;

    public Sprite 墙上的图片;
    public Sprite[] 浮现的图片2张;
    public float lastTimeAtLeast = 2.0f;

    private SpriteRenderer render;

    private GameObject player;
    private Controller control;
    private MouseLooker looker;

    private int spriteIndex;
    private bool isReading = false;
    private float mTimeLeft;

    [ContextMenu("Init")]
    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("OverlayCanvas").GetComponent<Canvas>();
        image = canvas.transform.Find("Panel/Image").GetComponent<Image>();
        text = canvas.transform.Find("Text").GetComponent<Text>();
        墙上的图片 = gameObject.GetComponent<SpriteRenderer>().sprite;
        if (浮现的图片2张 == null || 浮现的图片2张.Length == 0)
        {
            浮现的图片2张 = new Sprite[] { gameObject.GetComponent<SpriteRenderer>().sprite };
        }
    }

    void Start()
    {
        render = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        control = player.GetComponent<Controller>();
        looker = player.GetComponent<MouseLooker>();

        // 不显示图片
        render.sprite = null;
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
                    NextReading();
                    //FinishReading();
                }
            }
        }
	}

    public void StartReading()
    {
        spriteIndex = 0;
        Debug.LogWarning("StartReading");
        // Disable Mouse Move
        if (looker)
            looker.DisableMove();

        // Disable WASD Move
        if (control)
            control.enabled = false;

        NextReading();
    }

    IEnumerator DelayedReading(float sec, bool s)
    {
        yield return new WaitForSeconds(sec);
        isReading = s;
    }

    public void NextReading()
    {
        if (spriteIndex >= 浮现的图片2张.Length)
        {
            FinishReading();
            return;
        }
        // Replace Image
        image.sprite = 浮现的图片2张[spriteIndex];
        image.enabled = true;

        isReading = true;
        mTimeLeft = lastTimeAtLeast;

        // add sprite index
        spriteIndex++;
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
        render.sprite = 墙上的图片;
        //isReading = false;

        // Transition
        text.GetComponent<TransitionCommand>().Execute();
    }

    public bool GetReadingStatus()
    {
        return isReading;
    }
}
