using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class GlobalMusicPlayerSettings : ScriptableObject
{
	private static GlobalMusicPlayerSettings instance;

	public static GlobalMusicPlayerSettings Instance {
		get {
			if (instance == null)
				instance = Resources.Load<GlobalMusicPlayerSettings> ("global_music_player_settings");
			return instance;
		}
	}

	public AudioMixerGroup output;
}
