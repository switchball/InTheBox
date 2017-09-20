using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[ExecuteInEditMode()]
public class EasyDebugGuiButton : MonoBehaviour
{
	public bool drawInEditorMode = false;
	public GUISkin skin;
	public float fadeOutDelay = 3;
	public Rect position = new Rect (0, 0, 200, 50);
	public string label = "Easy Debug";
	public UnityEvent clicked;
	private float startRealTime = 0;

	void Start ()
	{
		startRealTime = Time.realtimeSinceStartup;
	}

	void OnGUI ()
	{
		if (!Application.isPlaying && Application.isEditor && !drawInEditorMode)
			return;

		if (skin != null)
			GUI.skin = skin;

		if (Application.isPlaying && fadeOutDelay > 0 && Time.realtimeSinceStartup - startRealTime > fadeOutDelay)
			GUI.color = new Color (1, 1, 1, 0);

		if (GUI.Button (position, label)) 
			clicked.Invoke ();
	}
}
