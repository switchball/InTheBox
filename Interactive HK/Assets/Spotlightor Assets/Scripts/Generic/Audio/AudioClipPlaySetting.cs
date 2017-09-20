using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

[System.Serializable]
public class AudioClipPlaySetting
{
	private static int selectedCandidateIndex = 0;

	public static int SelectedCandidateIndex {
		get { return selectedCandidateIndex; }
		set { selectedCandidateIndex = Mathf.Max (value, 0); }
	}

	public enum BonusClipsUsageTypes
	{
		Ignore,
		Random,
		Candidates,
	}

	public AudioClip ClipToPlay {
		get {
			AudioClip clipToPlay = defaultClip;
			if (bonusClipsUsage == BonusClipsUsageTypes.Random) {
				if (bonusClips.Length > 0) {
					int randomIndex = Random.Range (0, bonusClips.Length + 1);
					if (randomIndex > 0)
						clipToPlay = bonusClips [randomIndex - 1];
				}
			} else if (bonusClipsUsage == BonusClipsUsageTypes.Candidates) {
				int validIndex = Mathf.Clamp (SelectedCandidateIndex, 0, bonusClips.Length);
				if (validIndex > 0)
					clipToPlay = bonusClips [validIndex - 1];
			}
			return clipToPlay;
		}
	}

	[FormerlySerializedAs ("clip")]
	public AudioClip defaultClip;

	public BonusClipsUsageTypes bonusClipsUsage = BonusClipsUsageTypes.Ignore;
	public AudioClip[] bonusClips;

	[Range (0, 1), FormerlySerializedAs ("volume")]
	public float volumeScale = 1;
	[Range (-3, 3)]
	public float pitch = 1;

	public void SetupAudioSource (AudioSource audio)
	{
		audio.clip = ClipToPlay;
		audio.volume = volumeScale;
		audio.pitch = pitch;
	}
}
