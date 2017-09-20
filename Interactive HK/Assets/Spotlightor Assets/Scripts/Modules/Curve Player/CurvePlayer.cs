using UnityEngine;
using System.Collections;

public class CurvePlayer : MonoBehaviour
{
	public AnimationCurve curve = new AnimationCurve (new Keyframe (0, 0), new Keyframe (1, 1));

	public float speed = 1;
	public float startTime = 0;
	public bool ignoreTimeScale = false;

	public float Value{ get; private set; }

	void Awake ()
	{
		UpdateValue ();
	}

	void Update ()
	{
		UpdateValue ();
	}

	private void UpdateValue ()
	{
		float time = startTime;
		if (ignoreTimeScale)
			time += Time.unscaledTime * speed;
		else
			time += Time.time * speed;
		Value = curve.Evaluate (time); 
	}
}
