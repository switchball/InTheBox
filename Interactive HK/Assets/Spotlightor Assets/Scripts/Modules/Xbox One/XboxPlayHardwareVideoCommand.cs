using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

#if UNITY_XBOXONE
using HardwareVideo;
#endif

public class XboxPlayHardwareVideoCommand : CoroutineCommandBehavior
{
	public string movieFileName = "movie";
	public int movieWidth = 1920;
	public int movieHeight = 1080;
	public TextureDisplayer movieTextureDisplayer;
	#if UNITY_XBOXONE
	private bool playerInitialized = false;
	private bool playbackFinished = false;
	#endif

	#if UNITY_XBOXONE
	[DllImport ("HardwareVideo")]
	public static extern System.IntPtr GetHardwareVideoRenderEventCallback ();
	#endif

	void Awake ()
	{
		if (Application.platform != RuntimePlatform.XboxOne)
			gameObject.SetActive (false);
		else {
			#if UNITY_XBOXONE
			playerInitialized = HardwareVideoPlayer.Create (5, 0);
			#endif
		}
	}
	#if UNITY_XBOXONE
	protected override IEnumerator CoroutineCommand ()
	{
		if (playerInitialized) {
			if (SetupPlayback () == true) {
				StartCoroutine ("CallPluginAtEachEndOfFrame");

				playbackFinished = false;
				HardwareVideoPlayer.OnPlaybackFinished += HandlePlaybackFinished;
				HardwareVideoPlayer.Play (AudioSettings.dspTime);

				if (GetComponent<AudioSource> ())
					GetComponent<AudioSource> ().Play ();

				this.Log ("Video {0} start playing", movieFileName);

				while (!playbackFinished) {
					yield return null;
					HardwareVideoPlayer.Update (AudioSettings.dspTime);
				}

				HardwareVideoPlayer.OnPlaybackFinished -= HandlePlaybackFinished;
				StopCoroutine ("CallPluginAtEachEndOfFrame");

				this.Log ("Video {0} complete playing", movieFileName);
			} else {
				this.Log ("Failed to SetupPlayback");
			}
		} else
			this.Log ("Failed to initialize HardwareVideoPlayer");
	}

	private bool SetupPlayback ()
	{
		Texture2D tex = new Texture2D (movieWidth, movieHeight, TextureFormat.BGRA32, false);
		tex.filterMode = FilterMode.Point;
		tex.Apply ();

		movieTextureDisplayer.Display (tex);

		string assetLocation = System.IO.Path.Combine (Application.streamingAssetsPath, movieFileName);
		return HardwareVideoPlayer.InitPlayback (assetLocation, movieWidth, movieHeight, tex.GetNativeTexturePtr ());
	}

	private IEnumerator CallPluginAtEachEndOfFrame ()
	{
		while (true) {
			yield return new WaitForEndOfFrame ();
			GL.IssuePluginEvent (GetHardwareVideoRenderEventCallback (), 1337);
		}
	}


	void HandlePlaybackFinished ()
	{
		playbackFinished = true;
	}
	#else
	protected override IEnumerator CoroutineCommand ()
	{
		yield return null;
	}
	#endif
}
