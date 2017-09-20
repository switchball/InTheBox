using UnityEngine;
using System.Collections;

/// <summary>
/// Global Singleton BlackScreen.
/// Don't add it to scene. Call it from script directly.
/// </summary>
public class GlobalBlackScreenTransition : ColorScreenTransition
{
	private static GlobalBlackScreenTransition instance;

	public static GlobalBlackScreenTransition Instance {
		get {
			if (instance == null) {
				GameObject go = new GameObject ("Global Black Screen Transition");
				instance = go.AddComponent<GlobalBlackScreenTransition> ();
                //instance.outWhenAwake = false;
				instance.autoActivate = true;
				instance.durationIn = 0.5f;
				instance.durationOut = 0.4f;
				instance.delayOut = 0.1f;
                //instance.delayIn = 0.5f;
				instance.easeIn = instance.easeOut = iTween.EaseType.linear;
				instance.color = Color.black;
				instance.ignoreTimeScale = true;
				DontDestroyOnLoad (go);
			}
			return instance;
		}
	}

	protected override void Awake ()
	{
		if (GlobalBlackScreenTransition.instance != null && GlobalBlackScreenTransition.instance != this) {
			GameObject.Destroy (this);
		} else
			base.Awake ();
	}
}
