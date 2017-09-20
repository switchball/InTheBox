using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierPathOfArrayPoints : BezierPath
{
	private List<Vector3> points = new List<Vector3> ();

	public override Vector3[] Points {
		get { return points.ToArray (); }
	}

	public void AddPoint (Vector3 point)
	{
		points.Add (point);
	}

	public void RemovePointAt (int index)
	{
		points.RemoveAt (index);
	}

	public void ClearPoints ()
	{
		points.Clear ();
	}
}
