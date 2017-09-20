using UnityEngine;
using System;
using System.Collections;

#if UNITY_XBOXONE
using Users;
using Storage;
using DataPlatform;
using TextSystems;
#endif
public class XboxPluginsInitializer : MonoBehaviour
{
	public bool storage = true;
	public bool textSystems = true;
	public bool dataPlatform = true;
	public bool achievements = true;
	public TextAsset eventsManifest;
	public bool setupGetGamerPicRenderThread = true;
	#if UNITY_XBOXONE
	private static IntPtr usersRenderThreadCallback = IntPtr.Zero;
	private static bool initialized = false;
	#endif

	void Awake ()
	{
		if (Application.platform != RuntimePlatform.XboxOne)
			this.enabled = false;
	}

	#if UNITY_XBOXONE
	void Start ()
	{
		if (Application.platform == RuntimePlatform.XboxOne && !initialized) {
			
			if (storage)
				StorageManager.Create ();

			if (dataPlatform)
				DataPlatformPlugin.InitializePlugin (0);

			if (textSystems)
				TextSystemsManager.Create ();
			
			UsersManager.Create ();

			if (achievements)
				AchievementsManager.Create ();

			if (eventsManifest != null) {
				EventManager.CreateFromText (eventsManifest.text);
				eventsManifest = null;
			}

			if (setupGetGamerPicRenderThread) {
				// This is called when the UsersManager needs to push something to the render thread.
				// This is important for loading GamerPics as we upload the texture for you
				// and need to do this from the render thread. Providing this simple callback hookup
				// allows the plugin to push something to the render thread on need.
				//
				// If you fail to provide this GetGamerPic will never return!
				usersRenderThreadCallback = UsersManager.GetGamerPicRenderThreadEventRequestEventCallback ();
				UsersManager.OnRenderThreadEventRequestRequired += UsersManager_OnRenderThreadEventRequestRequired;
			}

			initialized = true;
		}
	}

	private static void UsersManager_OnRenderThreadEventRequestRequired (int eventId)
	{
		GL.IssuePluginEvent (usersRenderThreadCallback, eventId);
	}
	#endif
}
