using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CurvePlayerFloat : MonoBehaviour
{
	[System.Serializable]
	public class ValueUpdateEvent : UnityEvent<float>
	{
		
	}

	public CurvePlayer curvePlayer;
	public float valueFrom = 0;
	public float valueTo = 0;
	public ValueUpdateEvent onValueUpdate;

	void Update ()
	{
		onValueUpdate.Invoke (Mathf.LerpUnclamped (valueFrom, valueTo, curvePlayer.Value));
	}

	void Reset ()
	{
		if (curvePlayer == null)
			curvePlayer = GetComponent<CurvePlayer> ();
	}
}
