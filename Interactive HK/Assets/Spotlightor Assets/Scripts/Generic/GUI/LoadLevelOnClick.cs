using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GenericButton))]
public class LoadLevelOnClick : MonoBehaviour
{
	[Header ("Left blank to reload level")]
	public string
		levelName = "Home";

	void Start ()
	{
		GetComponent<GenericButton> ().Clicked += HandleClicked;
	}

	void HandleClicked (GenericButton button)
	{
		#if UNITY_5
		if (levelName != "")
			UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
		else
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		#else
		if (levelName != "")
			Application.LoadLevel (levelName);
		else
			Application.LoadLevel (Application.loadedLevel);
		#endif
	}
}
