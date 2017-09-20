using UnityEngine;
using System.Collections;

public class CustomCursorOnMouseOver : MonoBehaviour
{
	public bool hideMouseCursor;
	public Vector2 offset;
	public Texture cursorTexture;
	void OnMouseEnter ()
	{
		if (hideMouseCursor)
			#if UNITY_5
			Cursor.visible = false;
		#endif
		#if !UNITY_5
		Cursor.visible = false;
		#endif
		enabled = true;
	}

	void OnMouseExit ()
	{
		if (hideMouseCursor)
			#if UNITY_5
			Cursor.visible = true;
		#endif
		#if !UNITY_5
		Cursor.visible = true;
		#endif
		enabled = false;
	}

	void OnDisable ()
	{
		if (hideMouseCursor)
			#if UNITY_5
			Cursor.visible = true;
		#endif
		#if !UNITY_5
		Cursor.visible = true;
		#endif
	}

	void Start ()
	{
		enabled = false;
	}

	void OnGUI ()
	{
		if (cursorTexture) {
			Vector3 mousePos = Input.mousePosition;
			Rect drawRect = new Rect (mousePos.x + offset.x, Screen.height - mousePos.y + offset.y, cursorTexture.width, cursorTexture.height);
			GUI.DrawTexture (drawRect, cursorTexture);
		}
	}
}
