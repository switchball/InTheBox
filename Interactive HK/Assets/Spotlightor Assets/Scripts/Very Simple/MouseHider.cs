using UnityEngine;
using System.Collections;

public class MouseHider : MonoBehaviour
{
	public KeyCode showMouseKey = KeyCode.M;

	void Start ()
	{
		if (Application.isEditor == false)
#if UNITY_5
			Cursor.visible = false;
#endif
#if !UNITY_5
			Cursor.visible = false;
#endif
	}

	void Update ()
	{
		if (Input.GetKeyDown (showMouseKey))
			#if UNITY_5
			Cursor.visible = true;
		#endif
		#if !UNITY_5
			Cursor.visible = true;
		#endif
	}
}
