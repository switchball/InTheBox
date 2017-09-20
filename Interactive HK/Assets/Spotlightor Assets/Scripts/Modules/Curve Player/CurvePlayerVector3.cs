using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CurvePlayerVector3 : MonoBehaviour
{
	[System.Serializable]
	public class ValueUpdateEvent : UnityEvent<Vector3>
	{
		
	}

	public CurvePlayer curvePlayer;
	public Vector3 valueFrom = Vector3.zero;
	public Vector3 valueTo = Vector3.one;
	public ValueUpdateEvent onValueUpdate;

	void Update ()
	{
		onValueUpdate.Invoke (Vector3.LerpUnclamped (valueFrom, valueTo, curvePlayer.Value));
	}
}
