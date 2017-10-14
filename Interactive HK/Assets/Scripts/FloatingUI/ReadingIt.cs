using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[RequireComponent(typeof(SpriteRenderer))]
public class ReadingIt : MonoBehaviour {
    [Header("使用方法：拖到带有图片的组件上")]
    [Header("然后右击选择->Init，可自动完成部分设置")]
    public Canvas canvas;
    public Image panelImage;
    private TransitionManager panelImageTrans;
    private Image imageR2;
    private Image imageL1;
    public Text centerText;
    public Text cornerText;

    public Sprite 墙上的图片;
    public Sprite[] 浮现的图片2张;
    public float lastTimeAtLeast = 0.5f;

    public UnityEvent onFinished;

    private CommandBehavior command;
    private SpriteRenderer render;

    private GameObject player;
    private Controller control;
    private MouseLooker looker;

    private int spriteIndex;
    private bool isReading = false;
    private float mTimeLeft;

    private bool mHasRead = false;

    [ContextMenu("Init")]
    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("OverlayCanvas").GetComponent<Canvas>();
        panelImage = canvas.transform.Find("PanelDumb").GetComponent<Image>();
        panelImageTrans = canvas.transform.Find("PanelDumb").GetComponent<TransitionManager>();
        imageR2 = canvas.transform.Find("PanelR/Image").GetComponent<Image>();
        imageL1 = canvas.transform.Find("PanelL/Image").GetComponent<Image>();
        centerText = canvas.transform.Find("CenterText").GetComponent<Text>();
        cornerText = canvas.transform.Find("CornerText").GetComponent<Text>();
        command = panelImage.gameObject.GetComponent<CommandBehavior>();
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
        if (!s)
        {
            onFinished.Invoke();
            cornerText.GetComponent<TransitionCommand>().Execute();
        }
    }

    public void NextReading()
    {
        // Replace Image R
        if (spriteIndex < 浮现的图片2张.Length)
        {
            imageR2.enabled = true;
            imageR2.sprite = 浮现的图片2张[spriteIndex];
        } else
        {
            imageR2.enabled = false;
            imageR2.sprite = null;
        }
        // Replace Image L
        if (spriteIndex > 0 && spriteIndex <= 浮现的图片2张.Length)
        {
            imageL1.enabled = true;
            imageL1.sprite = 浮现的图片2张[spriteIndex-1];
        } else
        {
            imageL1.enabled = false;
            imageL1.sprite = null;
        }

        command.Execute();

        panelImageTrans.TransitionIn();
        //panelImage.enabled = true;

        isReading = true;
        mTimeLeft = lastTimeAtLeast;


        if (spriteIndex >= 浮现的图片2张.Length)
        {
            FinishReading();
            return;
        }
        // add sprite index
        spriteIndex++;
    }

    public void FinishReading()
    {
        Debug.LogWarning("EndReading");
        mHasRead = true;

        // Enable Mouse Move
        if (looker)
            looker.EnableMove();

        // Enable WASD Move
        if (control)
            control.enabled = true;

        // Disable Image
        panelImageTrans.TransitionOut();
        //panelImage.enabled = false;

        StartCoroutine(DelayedReading(lastTimeAtLeast, false));
        render.sprite = 墙上的图片;
        //isReading = false;

        // moved to DelayedReading(?, false)
        // Transition
        // cornerText.GetComponent<TransitionCommand>().Execute();

        // moved to DelayedReading(?, false)
        //onFinished.Invoke();
    }

    public bool GetReadingStatus()
    {
        return isReading;
    }

    public bool HasRead()
    {
        return mHasRead;
    }
}
