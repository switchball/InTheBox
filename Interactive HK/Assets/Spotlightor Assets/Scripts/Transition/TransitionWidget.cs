using UnityEngine;
using System.Collections;

[RequireComponent(typeof(WidgetTransitionManager))]
public abstract class TransitionWidget : MonoBehaviour
{
	public abstract void UpdateTransitionProgress (float progress);
}
