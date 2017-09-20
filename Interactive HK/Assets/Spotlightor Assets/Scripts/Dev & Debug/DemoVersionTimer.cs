using UnityEngine;
using System.Collections;
using System;

public class DemoVersionTimer : MonoBehaviour
{
	private static DemoVersionTimer instance = null;
	[StepperInt (1, 1985, int.MaxValue)]
	public int dueYear = 2015;
	[StepperInt (1, 1, 12)]
	public int dueMonth = 6;
	[StepperInt (1, 1, 31)]
	public int dueDay = 19;
	public float timeLimitInMinutes = 45;
	[TextArea ()]
	public string timeLimitReachedText = "DEMO TIME EXPIRED";
	private bool timeLimitReached = false;

	public DateTime DueDateTime {
		get { return new DateTime (dueYear, dueMonth, dueDay); }
	}

	void Start ()
	{
		if (instance == null) {
			instance = this;
			transform.SetParent (null);
			DontDestroyOnLoad (gameObject);

			StartCoroutine (DemoVersionTimeLimitCountDown ());
		} else
			Destroy (gameObject);
	}

	private IEnumerator DemoVersionTimeLimitCountDown ()
	{
		if (DueDateTime.CompareTo (DateTime.Now) > 0) {
			while (Time.realtimeSinceStartup / 60f < timeLimitInMinutes)
				yield return new WaitForSeconds (1);
		}
		
		timeLimitReached = true;
		Time.timeScale = 0;
	}

	void OnGUI ()
	{
		if (timeLimitReached) {
			if (GUI.Button (new Rect (0, Screen.height * 0.5f - 30, Screen.width, 60), timeLimitReachedText))
				Application.Quit ();
		}
	}
}
