using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationEventSoundPlayer : MonoBehaviour
{
	public List<AudioClip> clips;

	public void PlayClipAt (int clipIndex)
	{
		if (clips.Count > clipIndex)
			GlobalSoundPlayer.PlaySound (clips [clipIndex]);
	}
}
