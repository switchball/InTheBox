using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class XboxActivePlayers
{
	public const int MaxPlayersCount = 8;
	private static List<XboxPlayer> players;

	public static XboxPlayer First{ get { return Players [0]; } }

	public static List<XboxPlayer> Players {
		get {
			if (players == null) {
				players = new List<XboxPlayer> ();
				for (int i = 0; i < MaxPlayersCount; i++)
					players.Add (new XboxPlayer ());
			}
			return players;
		}
	}
}
