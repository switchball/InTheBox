using UnityEngine;
using System.Collections;

/// <summary>
/// Base component interface of COMPOSITE pattern.
/// </summary>
public interface IContentDisplayerComponent<T>
{
	void UpdateDisplay (T content);
}
