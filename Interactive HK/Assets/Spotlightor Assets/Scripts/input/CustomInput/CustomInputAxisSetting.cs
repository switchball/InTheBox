using UnityEngine;
using System.Collections;

[System.Serializable]
public class CustomInputAxisSetting
{
	public string axisName = "";
	public float sensitivity = 3;
	public float gravity = 3;
	public float dead = 0.001f;
	public bool snap = false;
	public KeyCode defaultPossitiveKey = KeyCode.None;
	public KeyCode defaultNegativeKey = KeyCode.None;
	public string altInputAxisName = "";
}
