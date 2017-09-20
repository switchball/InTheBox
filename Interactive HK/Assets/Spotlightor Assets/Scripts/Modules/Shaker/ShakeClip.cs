using UnityEngine;
using System.Collections;

public abstract class ShakeClip : ScriptableObject
{
	public float length = 0.5f;

	public virtual void OnPlayBy (ShakeSource source)
	{
	}

	public virtual void OnStopBy (ShakeSource source)
	{
	}

	public abstract Vector3 Sample (float time, ShakeSource source);
}
