using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ColorDisplayer))]
public class ColorTweenEffect : FunctionalMonoBehaviour
{
	[ColorUsage (true, true, 0, 8, 0.125f, 3)]
	public Color[] colors = new Color[]{ Color.white, new Color (1, 1, 1, 0) };
	public float speed = 1;
	public bool usHsbColorLerp = false;
	public bool ignoreTimeScale = false;
	private int startColorIndex = 0;
	private float progress = 0;
	private float lastUpdateRealTime = 0;
	private ColorDisplayer colorDisplayer;

	public Color StartColor {
		get { return colors [StartColorIndex]; }
	}

	public Color TargetColor {
		get {
			int targetColorIndex = StartColorIndex + 1;
			targetColorIndex %= colors.Length;
			return colors [targetColorIndex];		
		}
	}

	protected int StartColorIndex {
		get { return startColorIndex; }
		set {
			startColorIndex = value % colors.Length;
			if (startColorIndex < 0)
				startColorIndex += colors.Length;
		}
	}

	public ColorDisplayer ColorDisplayer {
		get {
			if (colorDisplayer == null) {
				colorDisplayer = GetComponent<ColorDisplayer> ();
				if (colorDisplayer == null)
					colorDisplayer = gameObject.AddComponent<ColorDisplayer> ();
			}
			return colorDisplayer;
		}
	}

	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		if (forTheFirstTime) {
			if (colors.Length == 0) {
				Debug.LogWarning ("ColorTweenEffects with 0 colors to tween! Auto remove the component.");
				Destroy (this);
			}
		}
		progress = 0;
	}

	protected override void OnBecameUnFunctional ()
	{
		TintColor (colors [0]);
	}

	void Update ()
	{
		float deltaTime = Time.deltaTime;
		if (ignoreTimeScale) {
			deltaTime = Time.realtimeSinceStartup - lastUpdateRealTime;
			lastUpdateRealTime = Time.realtimeSinceStartup;
		}
		progress += speed * deltaTime;
		Color color = usHsbColorLerp ? HsbColor.LerpColorByHsb (StartColor, TargetColor, progress) : Color.Lerp (StartColor, TargetColor, progress);
		TintColor (color);
		
		if (progress > 1) {
			progress = 0;
			StartColorIndex++;
		}
	}

	private void TintColor (Color color)
	{
		ColorDisplayer.Display (color);
	}
}
