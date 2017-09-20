using UnityEngine;
using System.Collections;

public class EasyConfigureFloatKeyboardSetter : MonoBehaviour
{
	public const float GuiHintDuration = 3;
	public string label = "Float Configure";
	public string key = "save key";
	public float modifyStep = 0.1f;
	public KeyCode increaseKey = KeyCode.UpArrow;
	public KeyCode decreaseKey = KeyCode.DownArrow;
	public GUISkin guiSkin;
	private float lastChangeTime = float.MinValue;

	void Update ()
	{
		float mod = 0;
		if (Input.GetKeyDown (increaseKey))
			mod = modifyStep;
		else if (Input.GetKeyDown (decreaseKey))
			mod = -modifyStep;
		if (mod != 0) {
			float value = EasyConfigure.GetFloat (key);
			value += mod;
			EasyConfigure.SetFloat (key, value);
			EasyConfigure.SaveFloat (key);

			lastChangeTime = Time.time;
		}
	}

	void OnGUI ()
	{
		if (Time.time - lastChangeTime < GuiHintDuration) {
			if (guiSkin != null)
				GUI.skin = guiSkin;
			GUI.Box (new Rect (Screen.width * 0.5f - 100, 10, 200, 30), string.Format ("{0} = {1}", label, EasyConfigure.GetFloat (key)));
		}
	}
}
