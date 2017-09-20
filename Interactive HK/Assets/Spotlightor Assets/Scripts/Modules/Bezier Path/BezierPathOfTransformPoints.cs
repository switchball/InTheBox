using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierPathOfTransformPoints : BezierPath
{
	public bool pointsByChildren = true;
	[HideByBooleanProperty ("pointsByChildren", true)]
	public List<Transform>
		transformPoints;
	public bool closed = false;
	private bool pointsByChildrenInitialized = false;

	public override Vector3[] Points {
		get {
			if (pointsByChildren && (!pointsByChildrenInitialized || (Application.isEditor && !Application.isPlaying))) {
				transformPoints = new List<Transform> (){ this.transform };
				for (int i = 0; i < transform.childCount; i++) {
					Transform childTransform = transform.GetChild (i);
					if (childTransform.gameObject.activeSelf)
						transformPoints.Add (childTransform);
				}
				pointsByChildrenInitialized = true;
			}

			int pointsCount = transformPoints.Count;
			if (closed)
				pointsCount++;
			Vector3[] points = new Vector3[pointsCount];
			for (int i = 0; i < transformPoints.Count; i++)
				points [i] = transformPoints [i].position;
			if (closed)
				points [pointsCount - 1] = transformPoints [0].position;

			return points;
		}
	}
}
