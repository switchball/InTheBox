using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BezierPath))]
[RequireComponent(typeof(LineRenderer))]
public class BezierPathLineRenderer : MonoBehaviour
{
	public float segementLength = 0.5f;
	public bool updateEachFrame = false;

	private BezierPath Path{ get { return GetComponent<BezierPath> (); } }

	private LineRenderer Line{ get { return GetComponent<LineRenderer> (); } }

	void Start ()
	{
		UpdateLineRenderer ();
	}

	void Update ()
	{
		if (updateEachFrame)
			UpdateLineRenderer ();
	}

	private void UpdateLineRenderer ()
	{
		float length = Path.EstimatedLength;
		int segementCount = Mathf.CeilToInt (length / segementLength) + 1;

		Line.SetVertexCount (segementCount + 1);
		for (int i = 0; i < segementCount+1; i++) {
			float progress = Mathf.InverseLerp (0, segementCount, i);
			Line.SetPosition (i, Path.GetPositionOnPath (progress));
		}
	}
}
