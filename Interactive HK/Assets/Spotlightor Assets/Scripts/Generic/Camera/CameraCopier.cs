using UnityEngine;
using System.Collections;

public class CameraCopier : MonoBehaviour
{
	[System.Serializable]
	public class SmoothChangeCopySourceSetting
	{
		public Camera sourceCamera;
		public float time = 1;
		public iTween.EaseType easeType = iTween.EaseType.easeInOutCubic;
	}
	public Camera destCamera;
	private Camera sourceCamera;

	public Camera SourceCamera { 
		get{ return sourceCamera;} 
		set {
			if (sourceCamera != value) {
				this.sourceCamera = value;
				transform.SetParent (value == null ? null : value.transform, true);
			}
		} 
	}

	public void SmoothChangeCopySource (SmoothChangeCopySourceSetting setting)
	{
		this.SourceCamera = null;

		StopCoroutine ("TweenCamera");
		StartCoroutine ("TweenCamera", setting);
	}

	private IEnumerator TweenCamera (SmoothChangeCopySourceSetting setting)
	{
		transform.SetParent (setting.sourceCamera.transform, true);

		Vector3 startLocalPosition = transform.localPosition;
		Quaternion startLocalRotation = transform.localRotation;
		float startDeltaFov = destCamera.fieldOfView - setting.sourceCamera.fieldOfView;

		Tweener tweener = new Tweener (0, 1, setting.time, setting.easeType);
		while (!tweener.IsCompleted) {
			yield return null;
			tweener.TimeElapsed += Time.deltaTime;

			transform.localPosition = Vector3.Lerp (startLocalPosition, Vector3.zero, tweener.Progress);
			transform.localRotation = Quaternion.Slerp (startLocalRotation, Quaternion.identity, tweener.Progress);
			destCamera.fieldOfView = Mathf.Lerp (startDeltaFov, 0, tweener.Progress) + setting.sourceCamera.fieldOfView;
		}

		this.SourceCamera = setting.sourceCamera;
	}

	void LateUpdate ()
	{
		if (SourceCamera != null) {
			transform.position = SourceCamera.transform.position;
			transform.rotation = SourceCamera.transform.rotation;
			destCamera.fieldOfView = SourceCamera.fieldOfView;
		}
	}

	void Reset ()
	{
		if (destCamera == null)
			destCamera = GetComponent<Camera> ();
		if (destCamera == null)
			destCamera = GetComponentInChildren<Camera> ();
		if (destCamera == null)
			destCamera = Camera.main;
	}
}
