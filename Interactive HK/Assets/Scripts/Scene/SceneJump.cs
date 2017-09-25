using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneJump : MonoBehaviour {

	public int sceneBuildIndex;
	public float timeDelayed = 0;

	public bool setSmallFinish = false;
	public bool setLargeFinish = false;

	public List<GameObject> toDestory;

	float acc = 0;
	bool active = false;

	int specialIndex = 1;

	void Update () {
		if (active)
			acc += Time.deltaTime;

		if (acc > timeDelayed) {

			// destroy something
			foreach(var s in toDestory) {
				Destroy (s.gameObject);
			}

			SceneManager.LoadScene (sceneBuildIndex);
		}
	}

	public void GoToNextScene () {
		active = true;
	}
}
