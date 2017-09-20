using UnityEngine;
using System.Collections;

public class LockFrameRate : MonoBehaviour
{
	public int frameRate = 30;
	public bool disableVsync = true;
	private int defaultVsync = 1;

	void Start ()
	{
		defaultVsync = QualitySettings.vSyncCount = 0;
	}

	void Update ()
	{
		Application.targetFrameRate = frameRate;
		if (disableVsync)
			QualitySettings.vSyncCount = 0;
		else
			QualitySettings.vSyncCount = defaultVsync;
	}

	void OnDisable ()
	{
		Application.targetFrameRate = -1;
		QualitySettings.vSyncCount = defaultVsync;
	}
}
