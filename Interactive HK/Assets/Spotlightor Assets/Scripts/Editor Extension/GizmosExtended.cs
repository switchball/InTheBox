using UnityEngine;
using System.Collections;

public static class GizmosExtended
{
	public static void DrawWireRing (Vector3 center, float radius)
	{
		DrawWireRing (center, radius, Vector3.up);
	}

	public static void DrawWireRing (Vector3 center, float radius, Vector3 up)
	{
		DrawWireRing (center, radius, up, 32);
	}

	public static void DrawWireRing (Vector3 center, float radius, Vector3 up, int segments)
	{
		Matrix4x4 preMatrix = Gizmos.matrix;
		Gizmos.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.FromToRotation (Vector3.up, up), Vector3.one);
		segments = Mathf.Max (segments, 4);
		float centralAngleStep = 360f / (float)segments;
		for (int i = 0; i < segments; i++) {
			float startCentralAngle = (float)i * centralAngleStep;
			float endCentralAngle = startCentralAngle + centralAngleStep;
			Vector3 startOffset = Quaternion.Euler (0, startCentralAngle, 0) * Vector3.forward * radius;
			Vector3 endOffset = Quaternion.Euler (0, endCentralAngle, 0) * Vector3.forward * radius;
			Gizmos.DrawLine (center + startOffset, center + endOffset);
		}
		Gizmos.matrix = preMatrix;
	}
}
