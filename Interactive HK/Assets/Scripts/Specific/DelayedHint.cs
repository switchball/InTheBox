using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class DelayedHint : MonoBehaviour
{
    public float mHintDelay;
    private float mDelay;
    public bool 开场时即计时 = true;
    public UnityEvent playEvent;
    public UnityEvent endEvent;

    private bool status; // has effect ?

    // Use this for initialization
    void Start()
    {
        if (开场时即计时)
            StartHint();
    }

    // Update is called once per frame
    void Update()
    {
        if (!status)
            return;
        mDelay -= Time.deltaTime;
        if (mDelay < 0)
        {
            PlayHint();
            status = false;
        }
    }

    public void PlayHint()
    {
        playEvent.Invoke();
    }

    public void StartHint()
    {
        mDelay = mHintDelay;
        status = true;
    }

    public void StopHint()
    {
        status = false;
        endEvent.Invoke();
    }
}
