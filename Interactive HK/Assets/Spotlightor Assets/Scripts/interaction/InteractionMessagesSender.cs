using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class InteractionMessagesSender : MonoBehaviour
{
	public class InteractionPointer
	{
		private bool isDown = false;
		private GameObject hoverGameObject;
		private GameObject downStartGameObject;

		public int Id{ get; private set; }

		public Vector2 ScreenPosition{ get; private set; }

		public InteractionPointer (int id)
		{
			this.Id = id;
		}

		public bool IsDown {
			get { return isDown; }
			private set {
				if (isDown != value) {
					isDown = value;
					if (isDown) {
						if (HoverGameObject != null) {
							downStartGameObject = HoverGameObject;
							SendPointerMessage (downStartGameObject, "OnPressDown");
						} 
					} else {
						if (downStartGameObject != null) {
							if (downStartGameObject == HoverGameObject)
								SendPointerMessage (downStartGameObject, "OnSelect");

							SendPointerMessage (downStartGameObject, "OnPressUp");
						}
						downStartGameObject = null;
					}
				}
			}
		}

		public GameObject HoverGameObject {
			get { return hoverGameObject; }
			private set {
				if (hoverGameObject != value) {
					if (hoverGameObject != null)
						SendPointerMessage (hoverGameObject, "OnHoverOut");

					hoverGameObject = value;
					if (value != null)
						SendPointerMessage (value, "OnHoverIn");
				}
			}
		}

		public GameObject DownStartGameObject{ get { return downStartGameObject; } }

		public void Update (InteractionPointerData pointerData, GameObject hitGameObject)
		{
			if (pointerData.id == this.Id) {
				this.ScreenPosition = pointerData.screenPosition;
				HoverGameObject = hitGameObject;
				IsDown = pointerData.isDown;

				if (HoverGameObject != null)
					SendPointerMessage (HoverGameObject, "OnHover");

				if (IsDown && downStartGameObject != null)
					SendPointerMessage (downStartGameObject, "OnDrag");
			} else
				Debug.LogWarning (string.Format ("Cannot update InteractionPointer({0}) with different id's data: {1}", this.Id, pointerData.id));
		}

		private void SendPointerMessage (GameObject target, string message)
		{
			PointerMessageData messageData = new PointerMessageData (this.Id, this.ScreenPosition);
			target.SendMessage (message, messageData, SendMessageOptions.DontRequireReceiver);
		}
	}

	public class PointerMessageData
	{
		public int pointerId = 0;
		public Vector2 pointerPosition = Vector2.zero;

		public PointerMessageData (int id, Vector2 position)
		{
			this.pointerId = id;
			this.pointerPosition = position;
		}
	}

	[System.Serializable]
	public class MessageCameraSetting
	{
		public Camera camera;
		public LayerMask raycastMask;

		public MessageCameraSetting (Camera camera, LayerMask raycastMask)
		{
			this.camera = camera;
			this.raycastMask = raycastMask;
		}
	}

	public class InteractionPointerData
	{
		public int id = 0;
		public bool isExisted;
		public bool isDown;
		public Vector2 screenPosition;

		public InteractionPointerData (int id, bool existed, bool down, Vector2 screenPosition)
		{
			this.id = id;
			this.isExisted = existed;
			this.isDown = down;
			this.screenPosition = screenPosition;
		}
	}
	private static ObjectInstanceFinder<InteractionMessagesSender> instanceFinder = new ObjectInstanceFinder<InteractionMessagesSender> ();

	public static InteractionMessagesSender Current{ get { return instanceFinder.Instance; } }

	public List<GraphicRaycaster> blockingGraphicRaycasters;
	private List<RaycastResult> blockingRaycastResults = new List<RaycastResult> ();
	private Dictionary<int, InteractionPointer> uiCursorByIds = new Dictionary<int, InteractionPointer> ();

	public bool UpdateUiCursor (InteractionPointerData pointerData, MessageCameraSetting raycastCameraSetting)
	{
		InteractionPointer uiCursor = null;
		if (!uiCursorByIds.TryGetValue (pointerData.id, out uiCursor)) {
			uiCursor = new InteractionPointer (pointerData.id);
			uiCursorByIds [pointerData.id] = uiCursor;
		}
		
		GameObject hitGameObject = null;
		if (pointerData.isExisted && !IsBlockedByGraphicRaycasters (pointerData)) {
			RaycastHit hit = new RaycastHit ();
			if (Physics.Raycast (raycastCameraSetting.camera.ScreenPointToRay (pointerData.screenPosition), out hit, Mathf.Infinity, raycastCameraSetting.raycastMask))
				hitGameObject = hit.transform.gameObject;

			if (hitGameObject != null && IsAnyOtherUiCusorAssosiateWithGameObject (hitGameObject, pointerData.id))
				hitGameObject = null;
		}

		if (!pointerData.isExisted)
			pointerData.isDown = false;

		uiCursor.Update (pointerData, hitGameObject);

		return hitGameObject != null;
	}

	private bool IsBlockedByGraphicRaycasters (InteractionPointerData pointerData)
	{
		bool blocked = false;
		if (blockingGraphicRaycasters.Count > 0) {
			PointerEventData pointerEventData = new PointerEventData (EventSystem.current);
			pointerEventData.position = pointerData.screenPosition;

			foreach (GraphicRaycaster raycaster in blockingGraphicRaycasters) {
				raycaster.Raycast (pointerEventData, blockingRaycastResults);
				if (blockingRaycastResults.Count > 0) {
					blocked = true;
					break;
				}
			}
			blockingRaycastResults.Clear ();
		}
		return blocked;
	}

	private bool IsAnyOtherUiCusorAssosiateWithGameObject (GameObject targetGameObject, int pointerId)
	{
		foreach (KeyValuePair<int, InteractionPointer> keyValuePair in uiCursorByIds) {
			if (keyValuePair.Key != pointerId) {
				InteractionPointer pointer = keyValuePair.Value;
				if (pointer.DownStartGameObject == targetGameObject || pointer.HoverGameObject == targetGameObject) {
					this.Log ("{0} is already belong to pointer {1}, won't react to pointer {2}!", targetGameObject.name, pointer.Id, pointerId);
					return true;
				}
			}
		}
		return false;
	}

	void Reset ()
	{
		if (GetComponent<InteractionMessagesSource> () == null)
			gameObject.AddComponent<MouseInteractionMessagesSource> ();
	}
}
