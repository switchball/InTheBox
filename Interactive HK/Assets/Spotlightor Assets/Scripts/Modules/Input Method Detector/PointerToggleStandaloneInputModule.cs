using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PointerToggleStandaloneInputModule : StandaloneInputModule
{
	private const float PointerEngageDeltaLength = 3;
	private bool pointerEnabled = true;
	private bool isPointerEngaged = false;

	public bool PointerEnabled {
		get { return pointerEnabled; }
		set {
			if (pointerEnabled != value) {
				pointerEnabled = value;

				if (pointerEnabled == false)
					ClearPointerData ();
				else
					IsPointerEngaged = false;
			}
		}
	}

	public bool IsPointerEngaged {
		get { return isPointerEngaged; }
		set {
			if (isPointerEngaged != value) {
				isPointerEngaged = value;
				if (!isPointerEngaged)
					ClearPointerData ();
			}
		}
	}

	public override bool IsModuleSupported ()
	{
		return true;
	}

	public override void Process ()
	{
		bool usedEvent = SendUpdateEventToSelectedObject ();

		if (eventSystem.sendNavigationEvents) {
			if (!usedEvent)
				usedEvent |= SendMoveEventToSelectedObject ();

			if (!usedEvent)
				SendSubmitEventToSelectedObject ();
		}

		if (PointerEnabled) {
			MouseState mouseData = GetMousePointerEventData ();
			MouseButtonEventData leftButtonData = mouseData.GetButtonState (PointerEventData.InputButton.Left).eventData;
			PointerEventData pointerEvent = leftButtonData.buttonData;

			if (IsPointerEngaged) {
				bool hasAxisInput = Input.GetButtonDown (horizontalAxis) || Input.GetButtonDown (verticalAxis);
				if (hasAxisInput)
					IsPointerEngaged = false;
			} else {
				if (pointerEvent.delta.magnitude > PointerEngageDeltaLength) 
					IsPointerEngaged = true;
			}

			if (IsPointerEngaged) {
				GameObject pointerEnterBefore = pointerEvent.pointerEnter;

				ProcessMouseEventNoDeselect (0);

				if (pointerEvent.pointerEnter != null && pointerEvent.pointerEnter != pointerEnterBefore) {
					GameObject pointerSelectGo = ExecuteEvents.GetEventHandler<ISelectHandler> (pointerEvent.pointerEnter);
					if (pointerSelectGo != null)
						eventSystem.SetSelectedGameObject (pointerSelectGo);
				}
			}
		}
	}

	protected void ProcessMouseEventNoDeselect (int id)
	{
		var mouseData = GetMousePointerEventData (id);
		var leftButtonData = mouseData.GetButtonState (PointerEventData.InputButton.Left).eventData;

		// Process the first mouse button fully
		ProcessMousePressNoDeselect (leftButtonData);
		ProcessMove (leftButtonData.buttonData);
		ProcessDrag (leftButtonData.buttonData);

		// Now process right / middle clicks
		ProcessMousePressNoDeselect (mouseData.GetButtonState (PointerEventData.InputButton.Right).eventData);
		ProcessDrag (mouseData.GetButtonState (PointerEventData.InputButton.Right).eventData.buttonData);
		ProcessMousePressNoDeselect (mouseData.GetButtonState (PointerEventData.InputButton.Middle).eventData);
		ProcessDrag (mouseData.GetButtonState (PointerEventData.InputButton.Middle).eventData.buttonData);

		if (!Mathf.Approximately (leftButtonData.buttonData.scrollDelta.sqrMagnitude, 0.0f)) {
			var scrollHandler = ExecuteEvents.GetEventHandler<IScrollHandler> (leftButtonData.buttonData.pointerCurrentRaycast.gameObject);
			ExecuteEvents.ExecuteHierarchy (scrollHandler, leftButtonData.buttonData, ExecuteEvents.scrollHandler);
		}
	}

	/// <summary>
	/// Process the current mouse press.
	/// </summary>
	protected void ProcessMousePressNoDeselect (MouseButtonEventData data)
	{
		var pointerEvent = data.buttonData;
		var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

		// PointerDown notification
		if (data.PressedThisFrame ()) {
			pointerEvent.eligibleForClick = true;
			pointerEvent.delta = Vector2.zero;
			pointerEvent.dragging = false;
			pointerEvent.useDragThreshold = true;
			pointerEvent.pressPosition = pointerEvent.position;
			pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

			//			DeselectIfSelectionChanged (currentOverGo, pointerEvent);

			// search for the control that will receive the press
			// if we can't find a press handler set the press
			// handler to be what would receive a click.
			var newPressed = ExecuteEvents.ExecuteHierarchy (currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);

			// didnt find a press handler... search for a click handler
			if (newPressed == null)
				newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler> (currentOverGo);

			// Debug.Log("Pressed: " + newPressed);

			float time = Time.unscaledTime;

			if (newPressed == pointerEvent.lastPress) {
				var diffTime = time - pointerEvent.clickTime;
				if (diffTime < 0.3f)
					++pointerEvent.clickCount;
				else
					pointerEvent.clickCount = 1;

				pointerEvent.clickTime = time;
			} else {
				pointerEvent.clickCount = 1;
			}

			pointerEvent.pointerPress = newPressed;
			pointerEvent.rawPointerPress = currentOverGo;

			pointerEvent.clickTime = time;

			// Save the drag handler as well
			pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler> (currentOverGo);

			if (pointerEvent.pointerDrag != null)
				ExecuteEvents.Execute (pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
		}

		// PointerUp notification
		if (data.ReleasedThisFrame ()) {
			// Debug.Log("Executing pressup on: " + pointer.pointerPress);
			ExecuteEvents.Execute (pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

			// Debug.Log("KeyCode: " + pointer.eventData.keyCode);

			// see if we mouse up on the same element that we clicked on...
			var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler> (currentOverGo);

			// PointerClick and Drop events
			if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick) {
				ExecuteEvents.Execute (pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
			} else if (pointerEvent.pointerDrag != null && pointerEvent.dragging) {
				ExecuteEvents.ExecuteHierarchy (currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
			}

			pointerEvent.eligibleForClick = false;
			pointerEvent.pointerPress = null;
			pointerEvent.rawPointerPress = null;

			if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				ExecuteEvents.Execute (pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);

			pointerEvent.dragging = false;
			pointerEvent.pointerDrag = null;

			// redo pointer enter / exit to refresh state
			// so that if we moused over somethign that ignored it before
			// due to having pressed on something else
			// it now gets it.
			if (currentOverGo != pointerEvent.pointerEnter) {
				HandlePointerExitAndEnter (pointerEvent, null);
				HandlePointerExitAndEnter (pointerEvent, currentOverGo);
			}
		}
	}

	protected void ClearPointerData ()
	{
		foreach (PointerEventData pointer in m_PointerData.Values) {
			// clear all selection
			HandlePointerExitAndEnter (pointer, null);
		}

		m_PointerData.Clear ();
	}
}
