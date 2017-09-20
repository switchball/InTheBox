using UnityEngine;
using System.Collections;

public class FpsDisplayer : MonoBehaviour
{
	public class FpsCalculator
	{
		private float updateInterval = 1;
		private float timeElapsedSinceLastUpdate = 0;
		private int frameCountSinceLastUpdate = 0;

		public float Fps{ get; private set; }

		public FpsCalculator (float updateInterval)
		{
			this.updateInterval = updateInterval;
		}

		public void AddFrame (float deltaTime)
		{
			frameCountSinceLastUpdate++;
			timeElapsedSinceLastUpdate += deltaTime;

			if (timeElapsedSinceLastUpdate >= updateInterval) {
				Fps = (float)frameCountSinceLastUpdate / timeElapsedSinceLastUpdate;

				frameCountSinceLastUpdate = 0;
				timeElapsedSinceLastUpdate = 0;
			}
		}
	}
	public int fontSize = 24;
	public Vector2 offset = new Vector2 (30, 30);
	private FpsCalculator fpsCalculatorShortTime;
	private FpsCalculator fpsCalculatorLongTime;

	void Start ()
	{
		fpsCalculatorShortTime = new FpsCalculator (0.5f);
		fpsCalculatorLongTime = new FpsCalculator (2);
	}

	void Update ()
	{
		fpsCalculatorShortTime.AddFrame (Time.deltaTime);
		fpsCalculatorLongTime.AddFrame (Time.deltaTime);
	}

	void OnGUI ()
	{
		GUIStyle style = new GUIStyle (GUI.skin.GetStyle ("Box"));
		style.fontSize = fontSize;
		style.wordWrap = false;
		style.alignment = TextAnchor.MiddleCenter;

		string fpsText = string.Format ("{0:00} / {1:00}", Mathf.RoundToInt (fpsCalculatorShortTime.Fps), Mathf.RoundToInt (fpsCalculatorLongTime.Fps));
		GUI.Box (new Rect (offset.x, offset.y, fontSize * 5, fontSize * 1.2f), fpsText, style);
	}
}
