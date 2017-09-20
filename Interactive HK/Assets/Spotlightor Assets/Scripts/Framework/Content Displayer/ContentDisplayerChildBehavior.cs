using UnityEngine;
using System.Collections;

public abstract class ContentDisplayerChildBehavior<T> : MonoBehaviour, IContentDisplayerComponent<T>
{
	public abstract void UpdateDisplay (T content);
}
