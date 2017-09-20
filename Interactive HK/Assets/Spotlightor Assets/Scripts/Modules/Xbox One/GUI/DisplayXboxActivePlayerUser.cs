using UnityEngine;
using System.Collections;

#if UNITY_XBOXONE
using Users;
#endif
[RequireComponent (typeof(XboxUserDisplayer))]
public class DisplayXboxActivePlayerUser : MonoBehaviour
{
	#if UNITY_XBOXONE
	public uint playerIndex = 0;
	[SingleLineLabel ()]
	public bool updateOnActivePlayerChange = true;
	private XboxUser displayingXboxUser;

	private XboxPlayer DisplayingPlayer {
		get { return XboxActivePlayers.Players [(int)playerIndex]; }
	}

	void OnEnable ()
	{
		UpdateDisplay ();

		if (updateOnActivePlayerChange)
			DisplayingPlayer.UserChanged += HandleXboxActivePlayerUserChanged;
		
		UsersManager.OnDisplayInfoChanged += HandleDisplayInfoChanged;
	}

	void OnDisable ()
	{
		DisplayingPlayer.UserChanged -= HandleXboxActivePlayerUserChanged;
		UsersManager.OnDisplayInfoChanged -= HandleDisplayInfoChanged;
	}

	void HandleXboxActivePlayerUserChanged (XboxPlayer xboxPlayer, int newUserId, int oldUserId)
	{
		UpdateDisplay ();
	}

	void HandleDisplayInfoChanged (int id)
	{
		this.Log ("User {0} display info changed", id);
		if (id == DisplayingPlayer.UserId) {
			this.Log ("Displaying user {0} displayInfo changed, update display.", id);
			UpdateDisplay ();
		}
	}

	private void UpdateDisplay ()
	{
		displayingXboxUser = new XboxUser (DisplayingPlayer.User);
		GetComponent<XboxUserDisplayer> ().UpdateDisplay (displayingXboxUser);
	}
	#endif
}
