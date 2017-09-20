using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BezierPath : MonoBehaviour
{
	public bool alwaysDrawGizmos = false;
	private float estimatedLength = -1f;
	
	public float EstimatedLength {
		get {
			if (estimatedLength == -1) {
				estimatedLength = 0;
				if (Points != null && Points.Length > 1) {
					for (int i = 1; i < Points.Length; i++) {
						estimatedLength += Vector3.Distance (Points [i], Points [i - 1]);
					}
				}
			}
			return estimatedLength;
		}
	}

	public abstract Vector3[] Points{ get; }

	public Vector3 GetPositionOnPath (float progress)
	{
		return iTween.PointOnPath (Points, Mathf.Clamp01 (progress));
	}
	
	public Quaternion GetLookRotationOnPath (float progress)
	{
		progress = Mathf.Clamp01 (progress);
		float fromPercent = progress;
		float toPercent = progress + 0.01f;
		if (toPercent > 1) {
			toPercent = 1;
			fromPercent = toPercent - 0.01f;
		}
		return Quaternion.LookRotation (GetPositionOnPath (toPercent) - GetPositionOnPath (fromPercent));
	}

	public Vector3 FindNearestPathPoint (Vector3 targetPosition)
	{
		float minDistance = float.MaxValue;
		float distance = 0;
		int nearestWaypointIndex = -1;
		for (int i = 0; i < Points.Length; i++) {
			Vector3 offset = targetPosition - Points [i];
			distance = offset.sqrMagnitude;
			if (distance < minDistance) {
				nearestWaypointIndex = i;
				minDistance = distance;
			}
		}
		return Points [nearestWaypointIndex];
	}
	
	public Vector3[] FindNearestPathSegment (Vector3 targetPosition)
	{
		Vector3[] linePoints = new Vector3[2];
		if (Points.Length < 2) {
			this.LogWarning ("Path points must be greater than 2");
			linePoints [0] = transform.position;
			linePoints [0] = transform.position;
		} else if (Points.Length == 2) {
			linePoints [0] = Points [0];
			linePoints [1] = Points [1];
		} else {
			float nearestDistance = float.MaxValue - 1;
			int nearestDistanceIndex = -1;
			float nextNearestDistance = float.MaxValue;
			int nextNearestDistanceIndex = -1;
			for (int i = 0; i <Points.Length; i++) {
				Vector3 pathPoint = Points [i];
				float distance = Vector3.Distance (pathPoint, targetPosition);
				if (distance < nearestDistance) {
					if (nearestDistance >= 0) {
						nextNearestDistanceIndex = nearestDistanceIndex;
						nextNearestDistance = nearestDistance;
					}
					nearestDistanceIndex = i;
					nearestDistance = distance;
				} else if (distance < nextNearestDistance) {
					nextNearestDistanceIndex = i;
					nextNearestDistance = distance;
				}
			}
			linePoints [0] = Points [Mathf.Min (nearestDistanceIndex, nextNearestDistanceIndex)];
			linePoints [1] = Points [Mathf.Max (nearestDistanceIndex, nextNearestDistanceIndex)];
		}
		
		return linePoints;
	}

	void OnDrawGizmosSelected ()
	{
		DrawGizmos ();
	}
	
	void OnDrawGizmos ()
	{
		if (alwaysDrawGizmos) 
			DrawGizmos ();
	}
	
	void DrawGizmos ()
	{
		if (Points != null && Points.Length >= 2) {
			int numCurveParts = Points.Length * 5;
			float step = (float)1 / numCurveParts;
			float progress = 0;
			Vector3 lineStart = Points [0];
			Vector3 lineEnd = Vector3.zero;
			Gizmos.color = Color.cyan;
			for (int i = 0; i < numCurveParts; i++) {
				progress += step;
				lineEnd = iTween.PointOnPath (Points, progress);
				Gizmos.DrawLine (lineStart, lineEnd);
				lineStart = lineEnd;
			}
		}
	}

}
