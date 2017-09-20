using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class CameraCrossFade : MonoBehaviour
{
	public Camera sourceCamera;
	public Camera targetCamera;
	public float time = 3;
	public iTween.EaseType easeType = iTween.EaseType.easeInOutCubic;
	public TextureRawImageOverlay.UiSettings uiSettings;

	public UnityEvent onCompleted;

	private TextureRawImageOverlay textureRawImageOverlay;

	void OnEnable ()
	{
		StartCoroutine ("CrossFade");
	}

	private IEnumerator CrossFade ()
	{
		SetupTempResources ();

		targetCamera.gameObject.SetActive (true);
		targetCamera.enabled = true;

		Tweener tweener = new Tweener (1, 0, time, easeType);
		do {
			yield return null;
			tweener.TimeElapsed += Time.deltaTime;
			textureRawImageOverlay.Opacity = tweener.Value;
		} while (!tweener.IsCompleted);
			
		sourceCamera.enabled = false;

		if (onCompleted != null) {
			onCompleted.Invoke ();
			yield return null;
		}

		this.enabled = false;
	}

	void OnDisable ()
	{
		StopCoroutine ("CrossFade");
		CleanUpTempResources ();
	}

	private void SetupTempResources ()
	{
		sourceCamera.targetTexture = new RenderTexture (Screen.width, Screen.height, 24);

		GameObject rawImageOverlayGo = new GameObject (name + "-CameraCrossFade Canvas");
		textureRawImageOverlay = rawImageOverlayGo.AddComponent<TextureRawImageOverlay> ();
		textureRawImageOverlay.texture = sourceCamera.targetTexture;
		textureRawImageOverlay.uiSettings = uiSettings;
	}

	private void CleanUpTempResources ()
	{
		if (sourceCamera != null && sourceCamera.targetTexture != null) {
			RenderTexture renderTexture = sourceCamera.targetTexture;
			sourceCamera.targetTexture = null;
			renderTexture.Release ();
		}

		if (textureRawImageOverlay != null)
			Destroy (textureRawImageOverlay.gameObject);
	}

	void Reset ()
	{
		if (sourceCamera == null)
			sourceCamera = GetComponent<Camera> ();
	}

	public class TextureRawImageOverlay : MonoBehaviour
	{
		[System.Serializable]
		public class UiSettings
		{
			public Material rawImageMaterial;
			[Tooltip ("HDR Tonemapped camera image is transparent. Increase overlay images count to remove the transparency.")]
			public int overlayImagesCount = 1;
			public RenderMode canvasRenderMode = RenderMode.ScreenSpaceOverlay;
			public string uiCameraTag = "UI Camera";
		}

		public Texture texture;
		public UiSettings uiSettings;

		private List<RawImage> rawImages;

		public float Opacity {
			set {
				rawImages.ForEach (image => image.color = new Color (1, 1, 1, value));
			}
		}

		void Start ()
		{
			gameObject.layer = 5; // UI layer

			Canvas canvas = gameObject.AddComponent<Canvas> ();
			canvas.renderMode = uiSettings.canvasRenderMode;
			if (uiSettings.canvasRenderMode == RenderMode.ScreenSpaceCamera) {
				GameObject cameraGo = GameObject.FindGameObjectWithTag (uiSettings.uiCameraTag);
				if (cameraGo != null) {
					canvas.worldCamera = cameraGo.GetComponent<Camera> ();
					if (canvas.worldCamera == null)
						this.LogWarning ("Cannot find Canvas camera with tag {0}", uiSettings.uiCameraTag);
				}
			}
			canvas.sortingOrder = -9999;

			CanvasScaler scaler = gameObject.AddComponent<CanvasScaler> ();
			scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

			rawImages = new List<RawImage> ();
			for (int i = 0; i < uiSettings.overlayImagesCount; i++)
				rawImages.Add (InstantiateRawImage ());
		}

		private RawImage InstantiateRawImage ()
		{
			RawImage rawImage;
			GameObject rawImageGo = new GameObject ("CameraCrossFade Image");
			rawImageGo.layer = 5; // UI layer

			RectTransform rectTransform = rawImageGo.AddComponent<RectTransform> ();
			rectTransform.SetParent (transform, false);
			rectTransform.anchorMax = Vector2.one;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchoredPosition3D = Vector3.zero;
			rectTransform.offsetMax = rectTransform.offsetMin = Vector2.zero;
			rectTransform.rotation = Quaternion.identity;
			rectTransform.localScale = Vector3.one;

			rawImageGo.AddComponent<CanvasRenderer> ();
			rawImage = rawImageGo.AddComponent<RawImage> ();
			rawImage.material = uiSettings.rawImageMaterial;
			rawImage.texture = texture;
			rawImage.raycastTarget = false;

			return rawImage;
		}
	}
}
