using UnityEngine;
using System.Collections;

public class ObjectDragger : MonoBehaviour
{

	private const string ProgressTweenName = "tween progress";
	public Camera viewCamera;
	public Rect dragViewportArea = new Rect (0.2f, 0.2f, 0.6f, 0.6f);
	public Transform dragTarget;
	public Vector3 targetMinPos;
	public Vector3 targetMaxPos;
	public float dragStrength = 12f;
	public float stopDeceleration = 3;
	public float borderResistance = 0.5f;
	public float borderRestoreStrength = 8;
	public iTween.EaseType tweenEase = iTween.EaseType.easeInOutQuart;
	public float tweenSpeed = 0.5f;
	private bool isDragging = false;
	private Vector2 dragStartOffset = Vector2.zero;
	private Vector2 dragStartFingerOffset = Vector2.zero;
	private Vector2 moveSpeed = Vector2.zero;
	private Vector2 offset = Vector2.zero;
	private Vector2 tweenStartOffset = Vector2.zero;
	private Vector2 tweenTargetOffset = Vector2.zero;

	private Camera ViewCamera {
		get { return viewCamera != null ? viewCamera : Camera.main;}
	}

	protected bool IsDragging {
		get { return isDragging; }
		set {
			if (isDragging != value) {
				isDragging = value;
				if (isDragging)
					StopTweenProgress ();
			}
		}
	}

	public Vector2 Progress {
		get { return new Vector2 (Offset.x / MaxOffset.x, Offset.y / MaxOffset.y);}
		set {
			Offset = new Vector2 (value.x * MaxOffset.x, value.y * MaxOffset.y);
			moveSpeed = Vector2.zero;
		}
	}

	private Vector2 Offset {
		get { return offset;}
		set {
			offset = value;
			dragTarget.localPosition = targetMinPos + new Vector3 (value.x, value.y, 0);
		}
	}
	
	private Vector2 MaxOffset {
		get { return new Vector2 (targetMaxPos.x - targetMinPos.x, targetMaxPos.y - targetMinPos.y);}
	}
	
	private Vector3 TargetMinWorldPosition {
		get { return dragTarget.parent == null ? targetMinPos : dragTarget.parent.TransformPoint (targetMinPos);}
	}
	
	private Vector3 TargetMaxWorldPosition {
		get { return dragTarget.parent == null ? targetMaxPos : dragTarget.parent.TransformPoint (targetMaxPos);}
	}

	private bool IsFingerDown {
		get {
			if (SystemInfo.deviceType == DeviceType.Handheld)
				return Input.touchCount > 0 && Input.touches [0].phase == TouchPhase.Began;
			else
				return Input.GetMouseButtonDown (0);
		}
	}
	
	private bool IsFingerUp {
		get {
			if (SystemInfo.deviceType == DeviceType.Handheld)
				return Input.touchCount == 0;
			else
				return Input.GetMouseButtonUp (0);
		}
	}
	
	private Vector2 CurrentFingerScreenPosition {
		get {
			if (SystemInfo.deviceType == DeviceType.Handheld && Input.touchCount > 0)
				return Input.touches [0].position;
			else
				return new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		}
	}

	public bool IsTweening {
		get { return iTween.Count (dragTarget.gameObject, "value") > 0; }
	}
	// Use this for initialization
	void Awake ()
	{
		Offset = ScreenPositionToOffset(ViewCamera.WorldToScreenPoint(dragTarget.position));
		//Offset = Vector2.zero;
		moveSpeed = Vector2.zero;
	}

	public void CenterToWorldPosInstantly(Vector3 worldPos)
	{
		Offset = CalculateCenterWolrdPosOffset(worldPos);
	}

	public void CenterToWorldPos (Vector3 worldPos)
	{
		tweenStartOffset = Offset;
		tweenTargetOffset = CalculateCenterWolrdPosOffset(worldPos);

		float time = (tweenTargetOffset - tweenStartOffset).magnitude / tweenSpeed;
		iTween.ValueTo (dragTarget.gameObject, iTween.Hash ("from", 0, "to", 1, "easetype", tweenEase, "time", time, 
		                                                    "onupdate", "OnTweenOffsetUpdate", "onupdatetarget", gameObject, "name", ProgressTweenName));
	}

	private Vector2 CalculateCenterWolrdPosOffset(Vector3 worldPos)
	{
		Vector3 centerScreenPos = ViewCamera.ViewportToScreenPoint (new Vector3 (0.5f, 0.5f, 0));
		Vector2 screenCenterOffset = ScreenPositionToOffset (centerScreenPos);
		Vector2 worldPosOffset = ScreenPositionToOffset (ViewCamera.WorldToScreenPoint (worldPos));
		Vector2 deltaMovement = screenCenterOffset - worldPosOffset;

		Vector2 result = deltaMovement+Offset;
		result.x = Mathf.Clamp (result.x, 0, MaxOffset.x);
		result.y = Mathf.Clamp (result.y, 0, MaxOffset.y);
		return result;
	}
	
	private void OnTweenOffsetUpdate (float progress)
	{
		this.Offset = Vector2.Lerp (tweenStartOffset, tweenTargetOffset, progress);
	}

	private void StopTweenProgress ()
	{
		iTween.StopByName (dragTarget.gameObject, ProgressTweenName);
	}

	void Update ()
	{
		Vector2 fingerPos = CurrentFingerScreenPosition;
		if (IsFingerDown) {
			Vector3 fingerViewportPos = ViewCamera.ScreenToViewportPoint (fingerPos);
			if (dragViewportArea.Contains (new Vector2 (fingerViewportPos.x, fingerViewportPos.y))) {
				IsDragging = true;
				dragStartOffset = Offset;
				dragStartFingerOffset = ScreenPositionToOffset (fingerPos);
				moveSpeed = Vector2.zero;
			}
		} else if (IsFingerUp)
			IsDragging = false;
		
		if (IsDragging && !IsTweening) {
			
			Vector2 deltaOffset = ScreenPositionToOffset (fingerPos) - dragStartFingerOffset;
			Vector2 targetOffset = deltaOffset + dragStartOffset;

			if (targetOffset.y > MaxOffset.y) {
				float dDistance = targetOffset.y - MaxOffset.y;
				dDistance *= 1f / (1f + dDistance * borderResistance);
				targetOffset.y = MaxOffset.y + dDistance;
			} else if (targetOffset.y < 0) {
				float dDistance = - targetOffset.y;
				dDistance *= 1f / (1f + dDistance * borderResistance);
				targetOffset.y = 0 - dDistance;
			}

			if (targetOffset.x > MaxOffset.x) {
				float dDistance = targetOffset.x - MaxOffset.x;
				dDistance *= 1f / (1f + dDistance * borderResistance);
				targetOffset.x = MaxOffset.x + dDistance;
			} else if (targetOffset.x < 0) {
				float dDistance = - targetOffset.x;
				dDistance *= 1f / (1f + dDistance * borderResistance);
				targetOffset.x = 0 - dDistance;
			}

			Vector2 newOffset = Vector2.zero;
			newOffset.x = Mathf.SmoothDamp (Offset.x, targetOffset.x, ref moveSpeed.x, 1f / dragStrength);
			newOffset.y = Mathf.SmoothDamp (Offset.y, targetOffset.y, ref moveSpeed.y, 1f / dragStrength);
			Offset = newOffset;
		} else {
			if (Offset.x >= 0 && Offset.x <= MaxOffset.x) {
				bool oldSpeedGreaterThanZero = moveSpeed.x > 0;
				moveSpeed.x -= stopDeceleration * moveSpeed.x * Time.deltaTime;
				bool newSpeedGreaterThanZero = moveSpeed.x > 0;
				if (oldSpeedGreaterThanZero != newSpeedGreaterThanZero)
					moveSpeed.x = 0;
				if (moveSpeed.x != 0) 
					Offset += new Vector2 (moveSpeed.x * Time.deltaTime, 0);
			} else {
				float targetDistance = Offset.x < 0 ? 0 : MaxOffset.x;
				Offset = new Vector2 (Mathf.SmoothDamp (Offset.x, targetDistance, ref moveSpeed.x, 1f / borderRestoreStrength), Offset.y);
			}

			if (Offset.y >= 0 && Offset.y <= MaxOffset.y) {
				bool oldSpeedGreaterThanZero = moveSpeed.y > 0;
				moveSpeed.y -= stopDeceleration * moveSpeed.y * Time.deltaTime;
				bool newSpeedGreaterThanZero = moveSpeed.y > 0;
				if (oldSpeedGreaterThanZero != newSpeedGreaterThanZero)
					moveSpeed.y = 0;
				if (moveSpeed.y != 0) 
					Offset += new Vector2 (0, moveSpeed.y * Time.deltaTime);
			} else {
				float targetDistance = Offset.y < 0 ? 0 : MaxOffset.y;
				Offset = new Vector2 (Offset.x, Mathf.SmoothDamp (Offset.y, targetDistance, ref moveSpeed.y, 1f / borderRestoreStrength));
			}
		}
	}
	
	private Vector2 ScreenPositionToOffset (Vector3 screenPos)
	{
		Vector3 worldPos = ViewCamera.ScreenToWorldPoint (screenPos);
		Vector3 posInDragTargetParent = dragTarget.parent == null ? worldPos : dragTarget.parent.InverseTransformPoint (worldPos);

		return new Vector2 (posInDragTargetParent.x - targetMinPos.x, posInDragTargetParent.y - targetMinPos.y);
	}
	
	void OnDrawGizmosSelected ()
	{
		float cameraDistance = ViewCamera.transform.InverseTransformPoint (transform.position).z;
		Vector3 topLeftViewportPos = new Vector3 (dragViewportArea.x, dragViewportArea.y, cameraDistance);
		Vector3 bottomRightViewportPos = new Vector3 (dragViewportArea.xMax, dragViewportArea.yMax, cameraDistance);
		Vector3 topRightViewportPos = new Vector3 (dragViewportArea.xMax, dragViewportArea.y, cameraDistance);
		Vector3 bottomLeftViewportPos = new Vector3 (dragViewportArea.x, dragViewportArea.yMax, cameraDistance);
		
		Vector3[] cornerPoints = new Vector3[4];
		cornerPoints [0] = ViewCamera.ViewportToWorldPoint (topLeftViewportPos);
		cornerPoints [1] = ViewCamera.ViewportToWorldPoint (topRightViewportPos);
		cornerPoints [2] = ViewCamera.ViewportToWorldPoint (bottomRightViewportPos);
		cornerPoints [3] = ViewCamera.ViewportToWorldPoint (bottomLeftViewportPos);
		for (int i = 0; i < cornerPoints.Length; i++) {
			Gizmos.DrawLine (cornerPoints [i], cornerPoints [i + 1 >= cornerPoints.Length ? 0 : i + 1]);
		}
		
	}
}
