using UnityEngine;
using System.Collections;

public class AudioCrossFader : MonoBehaviour
{
	private AudioSource activeAudioSource;
	private AudioSource nextAudioSource;
	private AudioClip currentAudioClip;
	private float activeAudioSourceVolumePercent = 1;
	private float volume = 1;
	
	public AudioClip CurrentAudioClip {
		get { return currentAudioClip;}
	}
	
	public float Volume {
		get {  return volume; }
		set {
			volume = value;
			ActiveAudioSourceVolumePercent = ActiveAudioSourceVolumePercent;
		}
	}
	
	private float ActiveAudioSourceVolumePercent {
		get { return activeAudioSourceVolumePercent;}
		set { 
			activeAudioSourceVolumePercent = Mathf.Clamp01 (value);
			activeAudioSource.volume = volume * activeAudioSourceVolumePercent;
			nextAudioSource.volume = volume * (1f - activeAudioSourceVolumePercent);
		}
	}
	
	public float Pitch {
		get { return activeAudioSource.pitch;}
		set { activeAudioSource.pitch = nextAudioSource.pitch = value; }
	}
#if UNITY_5
	public float Pan {
		get { return activeAudioSource.panStereo;}
		set { activeAudioSource.panStereo = nextAudioSource.panStereo = value; }
	}
#endif
#if !UNITY_5
	public float Pan {
		get { return activeAudioSource.panStereo;}
		set { activeAudioSource.panStereo = nextAudioSource.panStereo = value; }
	}
#endif
	
	public bool Loop {
		get { return activeAudioSource.loop;}
		set { activeAudioSource.loop = nextAudioSource.loop = value; }
	}
	
	protected virtual void Awake ()
	{
		activeAudioSource = gameObject.AddComponent<AudioSource> ();
		activeAudioSource.playOnAwake = false;
		activeAudioSource.Stop ();

		nextAudioSource = gameObject.AddComponent<AudioSource> ();
		nextAudioSource.playOnAwake = false;
		nextAudioSource.Stop ();

		ActiveAudioSourceVolumePercent = 1;
		Volume = 1;
	}
	
	public void CrossFadeTo (AudioClip clip, float fadeTime)
	{
		StopCoroutine ("DoCrossFadeTo");
		currentAudioClip = clip;
		StartCoroutine ("DoCrossFadeTo", fadeTime);
	}
	
	private IEnumerator DoCrossFadeTo (float fadeTime)
	{
		if (activeAudioSource.clip != currentAudioClip) {
			nextAudioSource.clip = currentAudioClip;
			if (nextAudioSource.clip != null)
				nextAudioSource.Play ();
			
			float timeElapsed = 0;
			while (timeElapsed < fadeTime) {
				yield return null;
				timeElapsed += Time.deltaTime;
				ActiveAudioSourceVolumePercent = 1f - timeElapsed / fadeTime;
			}
			ActiveAudioSourceVolumePercent = 0;
			if (activeAudioSource.clip != null) {
				activeAudioSource.Stop ();
				activeAudioSource.clip = null;
			}
			
			AudioSource tempAudioSource = activeAudioSource;
			activeAudioSource = nextAudioSource;
			nextAudioSource = tempAudioSource;
			
			ActiveAudioSourceVolumePercent = 1;
		}
	}
}
