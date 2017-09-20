using UnityEngine;
using System.Collections;

public class CustomCursorByTag : EnhancedMonoBehaviour
{
	[System.Serializable]
	public class CursorOfTag
	{
		public string tag;
		public Texture2D cursorTexture;
		public Vector2 hotspot;
		public CursorMode cursorMode = CursorMode.Auto;
	}
	
	public CursorOfTag[] cursorOfTags;
	public Camera viewCamera;
	public LayerMask mask;
	private CursorOfTag activeCursorOfTag;
	
	private CursorOfTag ActiveCursorOfTag {
		get { return activeCursorOfTag;}
		set {
			if (activeCursorOfTag != value) {
				activeCursorOfTag = value;
				if (activeCursorOfTag != null) 
					Cursor.SetCursor (value.cursorTexture, value.hotspot, value.cursorMode);
				else
					Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		Ray ray = viewCamera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Camera.main.farClipPlane, mask)) {
			string tag = hit.collider.gameObject.tag;
			ActiveCursorOfTag = FindCursorOfTag (tag);
		} else {
			ActiveCursorOfTag = null;
		}
	}
	
	CursorOfTag FindCursorOfTag (string tag)
	{
		foreach (CursorOfTag cot in cursorOfTags) {
			if (cot.tag == tag)
				return cot;
		}
		return null;
	}
	
	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		if (viewCamera == null)
			viewCamera = Camera.main;
	}
	
	protected override void OnBecameUnFunctional ()
	{
		ActiveCursorOfTag = null;
	}

	void OnDestroy ()
	{
		ActiveCursorOfTag = null;
	}
}
