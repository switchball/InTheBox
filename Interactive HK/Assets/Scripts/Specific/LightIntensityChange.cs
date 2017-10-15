using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Light))]
public class LightIntensityChange : MonoBehaviour {
    private Light light;
    private float targetIntensity;
    private GameObject player;
    public float speed = 1;
    public Vector2 zRange = new Vector2(138, 238);
    private float progress;

    public GameObject[] toDisapper;
    public UnityEvent onDisapper;

    private bool over = false;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        light = GetComponent<Light>();
        targetIntensity = light.intensity;
	}
	
	// Update is called once per frame
	void Update () {
        if (over || player.transform.position.x >= zRange.y + 1.4f)
        {
            if (!over)
            {
                light.intensity = 20f;
                onDisapper.Invoke();
                foreach( var g in toDisapper)
                    g.SetActive(false);
            }
            over = true;
            targetIntensity = 0.1f;
            speed = 0.3f;
            
        } else
        {
            progress = Mathf.InverseLerp(zRange.x, zRange.y, player.transform.position.x);
            if (progress > 0.1f)
            {
                if (progress < 0.5f)
                {
                    // 0.5 -> 10
                    targetIntensity = Mathf.Lerp(2.5f, 17.5f, progress);
                }
                else if (progress < 0.8f)
                {
                    // 0.8 -> 20
                    targetIntensity = 28.0f * Mathf.Pow(progress, 1.5f);
                } else
                {
                    // 0.8 -> 40
                    targetIntensity = 4f / (1.02f - progress);
                    speed = 20f;
                }
            }
        }

        light.intensity = Mathf.Lerp(light.intensity, targetIntensity, Time.deltaTime * speed);
	}

    public void ChangeToIntensity(float t)
    {
        targetIntensity = t;
    }
}
