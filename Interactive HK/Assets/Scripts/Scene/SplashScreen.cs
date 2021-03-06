﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    //模式id
    public int modeID = 0;
    //要加载的关卡
    public int LevelToLoad = -1;
    //Logo贴图
    public Texture2D SplashLogo;
    //Logos贴图
    public Texture2D[] SplashLogoList;
    //渐入渐出速度
    public float FadeSpeed = 0.3F;
    //等待时间
    public float WaitTime = 0.5F;

    public float DelayTime = 2F;
    #region 渐入渐出的类型
    public enum SplashType
    {
        LoadLevelThenFadeOut,
        FadeOutThenLoadLevel
    }
    public SplashType Type = SplashType.LoadLevelThenFadeOut;
    #endregion

    #region 渐入渐出的状态
    public enum FadeStatus
    {
        FadeIn,
        FadeWait,
        FadeOut
    }
    private FadeStatus mStatus = FadeStatus.FadeIn;
    #endregion

    //是否允许玩家触发渐入渐出事件
    public bool WaitForInput = true;
    //当前透明度
    private float mAlpha = 0.0F;
    //摄像机
    private Camera mCam;
    private GameObject mCamObj;
    //Logo贴图位置
    private Rect mSplashLogoPos;
    //渐入结束的时间
    private float mFadeInFinishedTime;

    private float mDelayTimeLeft;
    //关卡是否加载完毕
    private bool LevelisLoaded = false;

    private bool flag = true;

    public bool 主动预加载下一场景 = true;
    private AsyncOperation async;   //定义异步加载

    void Start()
    {
        if (主动预加载下一场景)
            StartPreLoad();

        mDelayTimeLeft = DelayTime;
        //保存相机
        mCam = Camera.main;
        mCamObj = Camera.main.gameObject;
        if (SplashLogo != null)
        {
            //计算Logo绘制的位置
            mSplashLogoPos.x = (Screen.width * 0.5F - SplashLogo.width * 0.5F);
            mSplashLogoPos.y = (Screen.height * 0.5F - SplashLogo.height * 0.5F);
            mSplashLogoPos.width = SplashLogo.width;
            mSplashLogoPos.height = SplashLogo.height;
        }
        if (SplashLogo != null && SplashLogoList.Length == 0)
        {
            SplashLogoList = new Texture2D[1];
            SplashLogoList[0] = SplashLogo;
            modeID = 0;
        }
        //如果是渐出后加载关卡则保留相机
        if (Type == SplashType.LoadLevelThenFadeOut)
        {
            //DontDestroyOnLoad(this);
            //DontDestroyOnLoad(Camera.main);
        }
        //检查目标关卡是否为空
        if ((Application.levelCount <= 1) || (LevelToLoad == -1))
        {
            Debug.Log("There is not have the level to load please check again");
            return;
        }
    }

    void Update()
    {
        if (flag)
            return;

        mDelayTimeLeft -= Time.deltaTime;
        if (mDelayTimeLeft > 0)
            return;

        switch (mStatus)
        {
            case FadeStatus.FadeIn:
                mAlpha += FadeSpeed * Time.deltaTime;
                break;
            case FadeStatus.FadeOut:
                mAlpha -= FadeSpeed * Time.deltaTime;
                break;
            case FadeStatus.FadeWait:
                //当设定为FadeWait时可根据时间判定或者玩家触发进入下一个状态
                if ((!WaitForInput && Time.time > mFadeInFinishedTime + WaitTime) || (WaitForInput && Input.anyKey))
                {
                    mStatus = FadeStatus.FadeOut;
                }
                break;
        }

    }

    void OnGUI()
    {
        if (async != null)
        {
            int progress = (int)(async.progress * 100);
            //GUI.Label(new Rect(10, 10, 300, 60), "Async Loading Progress:" + progress);
        }


        if (flag)
            return;

        if (mDelayTimeLeft <= 0)
        {
            //绘制Logo
            if (SplashLogo != null)
            {
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, Mathf.Clamp(mAlpha, 0, 1));
                GUI.DrawTexture(mSplashLogoPos, SplashLogo);
            }

            //绘制logo list中的logo
            if (modeID < SplashLogoList.Length)
            {
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, Mathf.Clamp(mAlpha, 0, 1));
                Rect pos = new Rect();
                Texture2D logo = SplashLogoList[modeID];
                float ratio = 1.0f * logo.width / logo.height;
                if (1.0 * Screen.width / Screen.height >= ratio)
                {
                    pos.width = Screen.width;
                    pos.height = (float)(1.0 * Screen.width / ratio);
                } else
                {
                    pos.height = Screen.height;
                    pos.width = (float)(Screen.height * ratio);
                }
                pos.x = (Screen.width * 0.5F - pos.width * 0.5F);
                pos.y = (Screen.height * 0.5F - pos.height * 0.5F);
                GUI.DrawTexture(pos, logo);
            } else
            {
                Debug.LogWarning("logo list not set for scene mode=" + (modeID + 1));
            }

            //状态判断
            if (mAlpha > 1.0F)
            {
                mStatus = FadeStatus.FadeWait;
                mFadeInFinishedTime = Time.time;
                mAlpha = 1.0F;
                //如果需要在渐入结束后加载关卡
                if (Type == SplashType.LoadLevelThenFadeOut)
                {
                    Debug.LogWarning("LoadLevelThenFadeOut");
                    mCam.depth = -1000;
                    JumpNextScene();
                }
            }

            if (mAlpha < 0.0F)
            {
                //如果需要在关卡加载完后渐出
                if (Type == SplashType.FadeOutThenLoadLevel)
                {
                    Debug.LogWarning("FadeOutThenLoadLevel");

                    //StartCoroutine(LoadSence());
                }
                else
                {
                    Destroy(mCamObj);
                    Destroy(this);
                }
            }
        }

        if (mStatus == FadeStatus.FadeWait)
        {
            mStatus = FadeStatus.FadeOut;
            Debug.LogWarning("FadeWait");

            //StartCoroutine(LoadSence());
            //Debug.Log("请按任意键继续");   
        }

        
    }

    void OnLevelWasLoaded(int index)
    {
        //如果目标关卡已加载需要手动销毁场景中的GUI和AudioListener
        if (LevelisLoaded)
        {
            //Destroy(mCam.GetComponent<AudioListener>());
            //Destroy(mCam.GetComponent<GUILayer>());
        }
    }

    public void SetModeID(int mode)
    {
        modeID = mode - 1;
    }

    public void StartSplash()
    {
        flag = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartSplash();
        }
    }

    public void JumpNextScene()
    {
        Debug.Log("JumpNextScene");
        if (async == null)
        {
            if (LevelToLoad != -1)
                SceneManager.LoadScene(LevelToLoad);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            async.allowSceneActivation = true;
        }
    }

    public void StartPreLoad() {
        Debug.Log("Preload start!");
        StartCoroutine(ScenePreLoad());
    }


    IEnumerator ScenePreLoad()
    {
        yield return new WaitForSeconds(1.0f);
        if (LevelToLoad != -1)
            async = SceneManager.LoadSceneAsync(LevelToLoad);
        else
            async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        async.allowSceneActivation = false;//场景暂时不进入
        Debug.Log("Here");
        yield return async;
    }

    //激活场景
    public void ActivatedScene()
    {
        async.allowSceneActivation = true;
    }
}