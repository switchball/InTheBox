using UnityEngine;
using System.Collections;

public abstract class ClampRange<T>
{
	public T min;
	public T max;
	
	public ClampRange (T min, T max)
	{
		this.min = min;
		this.max = max;
	}

	public abstract T Clamp (T value);

	public abstract bool Contains(T value);
}
