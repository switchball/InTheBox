using UnityEngine;
using System.Collections;

#if UNITY_XBOXONE
using Users;
#endif

public class XboxSinglePlayerSaveEngineInstaller : MonoBehaviour
{
	public XboxOneSaveEngine xboxSaveEngine;
	#if UNITY_XBOXONE

	private XboxPlayer Player{ get { return XboxActivePlayers.First; } }

	void Start ()
	{
		if (Application.platform == RuntimePlatform.XboxOne) {
			XboxOnePLM.OnSuspendingEvent += HandleSuspendingEvent;
			XboxOnePLM.OnResumingEvent += HandleResumingEvent;
		}
	}

	void OnDestroy ()
	{
		if (Application.platform == RuntimePlatform.XboxOne) {
			XboxOnePLM.OnSuspendingEvent -= HandleSuspendingEvent;
			XboxOnePLM.OnResumingEvent -= HandleResumingEvent;
		}
	}

	void HandleSuspendingEvent ()
	{
		this.enabled = false;
		this.Log ("Suspending, stop watching for active player save engine installation");
	}

	void HandleResumingEvent (double secondsSuspended)
	{
		this.enabled = true;
		this.Log ("Resume from suspend after {0} seconds, start watching for active player save engine installation", secondsSuspended);
	}

	// Fore updating after XboxPlayer.ControllerUserPairValidator.Update(), in order to make sure the IsControllerUserPairValid is latest.
	void LateUpdate ()
	{
		if (Application.platform == RuntimePlatform.XboxOne) {
			// 只保证ActivePlayer有效时，对应UserID的engine被正确安装。但不会在ActivePlayer无效时卸载Engine，因为如果只是因为手柄断开就卸载，那就不合理了。
			if (Player.IsControllerUserPairValid) {
				if (xboxSaveEngine.UserId == Player.UserId) {
					if (xboxSaveEngine.State == SaveEngine.StateTypes.NotInstalled) {
						this.Log ("SaveEngine.UserId = ActivePlayer.UserId = {0}, but not installed. Intall.", xboxSaveEngine.UserId);
						xboxSaveEngine.Install ();
					}
				} else {
					if (xboxSaveEngine.State == SaveEngine.StateTypes.NotInstalled) {
						this.Log ("NotInstalled SaveEngine.UserId {0} != ActivePlayer.UserId {1}. Intall.", xboxSaveEngine.UserId, Player.UserId);
						xboxSaveEngine.UserId = Player.UserId;
						xboxSaveEngine.Install ();
					} else if (xboxSaveEngine.State == SaveEngine.StateTypes.Installed) {
						this.Log ("Installed SaveEngine.UserId {0} != ActivePlayer.UserId {1}. Uninstall.", xboxSaveEngine.UserId, Player.UserId);
						xboxSaveEngine.Uninstall ();
					}
				}
			}
		}
	}
	#endif
}
