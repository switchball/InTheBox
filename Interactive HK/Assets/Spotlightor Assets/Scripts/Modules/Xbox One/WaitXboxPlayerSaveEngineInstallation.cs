using UnityEngine;
using System.Collections;

public class WaitXboxPlayerSaveEngineInstallation : CoroutineCommandBehavior
{
	public int playerIndex = 0;
	public XboxOneSaveEngine saveEngine;

	protected override IEnumerator CoroutineCommand ()
	{
		XboxPlayer player = XboxActivePlayers.Players [playerIndex];

		this.Log ("Wait Xbox Save Engine {0} installed for XboxPlayer {1}", saveEngine, player);
		while ((saveEngine.State == XboxOneSaveEngine.StateTypes.Installed && player.UserId == saveEngine.UserId) == false)
			yield return null;

		this.Log ("Wait installation completed.");
	}
}
