using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(InteractionMessagesSender))]
public abstract class InteractionMessagesSource : MonoBehaviour
{
	public List<InteractionMessagesSender.MessageCameraSetting> messageCameraSettings;
	private InteractionMessagesSender sender;
	
	public InteractionMessagesSender Sender {
		get {
			if (sender == null)
				sender = GetComponent<InteractionMessagesSender> ();
			return sender;
		}
	}

	void Awake ()
	{
		messageCameraSettings.ForEach (cameraSetting => cameraSetting.camera.eventMask = 0);
	}

	void Update ()
	{
		List<InteractionMessagesSender.InteractionPointerData> uiCursorDatas = GetUiCursorDatas ();
		foreach (InteractionMessagesSender.InteractionPointerData data in uiCursorDatas) {
			foreach (InteractionMessagesSender.MessageCameraSetting cameraSetting in messageCameraSettings) {
				bool hit = Sender.UpdateUiCursor (data, cameraSetting);
				if (hit) // 1 cursor get only 1 hit in all camera settings (no penetrate)
					break;
			}
		}
	}

	protected abstract List<InteractionMessagesSender.InteractionPointerData> GetUiCursorDatas ();
}
