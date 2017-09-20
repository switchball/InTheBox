using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HistoryMotionDebugLine : MonoBehaviour
{
	public Transform target;
	public float historyRecordTime = 1;
	public Color color = Color.blue;
	private List<Vector3> historyPositions = new List<Vector3> ();
	private float recordedTime = 0;

	void Start ()
	{
		if (target == null)
			target = this.transform;
	}

	void Update ()
	{
		historyPositions.Add (target.position);
		recordedTime += Time.deltaTime;

		while (recordedTime > historyRecordTime) {
			historyPositions.RemoveAt (0);
			recordedTime -= Time.deltaTime;
		}

		for (int i = 1; i < historyPositions.Count; i++)
			Debug.DrawLine (historyPositions [i - 1], historyPositions [i], color);
	}
}
