using UnityEngine;
using System.Collections;

public class SmartUnityGuiLabel : MonoBehaviour , ITransition
{

	public enum PositionMethod
	{
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight,
		
		TopSmart,
		BottomSmart,
		Smart,
		Top,
		Bottom,
	}
	
	private const float LazyRange = 50;// in pixels
	
	public string text = "LABEL";
	public GUISkin guiSkin;
	public string styleName = "Tooltip";
	public Color tintColor = Color.white;
	public PositionMethod positionMethod = PositionMethod.Smart;
	public float offsetX;
	public float offsetY;
	public float transitionTime = 0.3f;
	private Vector3 screenDrawPosition = Vector3.zero;
	private float leftRight = 0;
	private float topBottom = 0;
	
	public bool DrawAtRight {
		get { return leftRight == 1;}
	}
	
	public bool DrawAtBottom {
		get { return topBottom == 1;}
	}
	
	public GUIStyle LabelStyle { get { return guiSkin.GetStyle (styleName); } }
	
	public void UpdateDrawPositionInWorld (Vector3 worldPosition)
	{
		Vector3 screenPos = Camera.main.WorldToScreenPoint (worldPosition);
		UpdateDrawPositionOnScreen (screenPos);
	}
	
	public void UpdateDrawPositionOnScreen (Vector3 screenPosition)
	{
		this.screenDrawPosition = screenPosition;
		switch (positionMethod) {
		case PositionMethod.Smart:
			MakeSureDrawUpOrDown (screenDrawPosition);
			MakeSureDrawLeftOrRight (screenDrawPosition);
			break;
		case PositionMethod.TopSmart:
			MakeSureDrawLeftOrRight (screenDrawPosition);
			topBottom = 0;
			break;
		case PositionMethod.BottomSmart:
			MakeSureDrawLeftOrRight (screenDrawPosition);
			topBottom = 1;
			break;
		case PositionMethod.BottomLeft:
			topBottom = 1;
			leftRight = 0;
			break;
		case PositionMethod.BottomRight:
			topBottom = 1;
			leftRight = 1;
			break;
		case PositionMethod.TopLeft:
			topBottom = 0;
			leftRight = 0;
			break;
		case PositionMethod.TopRight:
			topBottom = 0;
			leftRight = 1;
			break;
		case PositionMethod.Top:
			topBottom = 0;
			leftRight = 0.5f;
			break;
		case PositionMethod.Bottom:
			topBottom = 1;
			leftRight = 0.5f;
			break;
		}
	}
	
	protected void MakeSureDrawLeftOrRight (Vector2 screenPosition)
	{
		if (DrawAtRight && screenPosition.x > 0.5f * Screen.width + LazyRange)
			leftRight = 0;
		else if (!DrawAtRight && screenPosition.x < 0.5f * Screen.width - LazyRange)
			leftRight = 1;
	}
	
	protected void MakeSureDrawUpOrDown (Vector2 screenPosition)
	{
		if (DrawAtBottom && screenPosition.y < 0.5f * Screen.height - LazyRange)
			topBottom = 0;
		else if (!DrawAtBottom && screenPosition.y > 0.5f * Screen.height + LazyRange)
			topBottom = 1;
	}

	void OnGUI ()
	{
		GUIStyle myLabelStyle = LabelStyle;
		if (tintColor.a > 0 && myLabelStyle != null && text != null && text != "") {
			if (screenDrawPosition.x < 0 || screenDrawPosition.y < 0 || screenDrawPosition.x > Screen.width || screenDrawPosition.y > Screen.height || screenDrawPosition.z < 0)
				return;

			GUI.color = tintColor;
			
			GUIContent content = new GUIContent (text);
			
			float contentWidthMax = myLabelStyle.fixedWidth;
			float contentWidthMin = 0;
			float contentHeight = myLabelStyle.CalcHeight (content, contentWidthMax);

			myLabelStyle.CalcMinMaxWidth (content, out contentWidthMin, out contentWidthMax);
			float labelHeight = contentHeight + LabelStyle.overflow.top + LabelStyle.overflow.bottom;
			float labelWidth = contentWidthMax + LabelStyle.border.left + LabelStyle.border.right + LabelStyle.overflow.left + LabelStyle.overflow.right;
			
			float drawX = this.screenDrawPosition.x;
			float drawY = Screen.height - this.screenDrawPosition.y;
			drawX += Mathf.Lerp (offsetX, -labelWidth - offsetX, leftRight);
			drawY += Mathf.Lerp (offsetY, -labelHeight - offsetY, 1 - topBottom);

			Rect drawRect = new Rect (drawX, drawY, labelWidth, labelHeight);
			GUI.Label (drawRect, content, myLabelStyle);
		}
	}
	
	void OnDisable ()
	{
		TransitionOut (true);
	}
	
	#region ITransition implementation
	public void TransitionIn ()
	{
		TransitionIn (false);
	}

	public virtual void TransitionIn (bool instant)
	{
		if (instant) {
			tintColor.a = 1;
		} else {
			StopCoroutine ("TweenAlpha");
			StartCoroutine ("TweenAlpha", 1);
		}
		enabled = true;
	}

	public void TransitionOut ()
	{
		TransitionOut (false);
	}

	public virtual void TransitionOut (bool instant)
	{
		if (instant) {
			tintColor.a = 0;
			enabled = false;
		} else {
			StopCoroutine ("TweenAlpha");
			StartCoroutine ("TweenAlpha", 0);
		}
	}
	#endregion
	
	IEnumerator TweenAlpha (float targetAlpha)
	{
		float startAlpha = tintColor.a;
		float duration = Mathf.Abs (targetAlpha - startAlpha) * transitionTime;
		float timeElapsed = 0;
		while (timeElapsed < duration) {
			yield return null;
			timeElapsed += Time.deltaTime;
			float t = Mathf.Clamp01 (timeElapsed / duration);
			tintColor.a = Mathf.Lerp (startAlpha, targetAlpha, t);
		}
		if (tintColor.a == 0)
			enabled = false;
	}
}
