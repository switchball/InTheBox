using UnityEngine;
using System.Collections;

public class MapPositionBetweenCameras : MonoBehaviour
{
	[Header("Auto set to MainCamera if NULL")]
	public Camera
		sourceCamera;
	[Header("Auto set to parent Camera if NULL")]
	public Camera
		destCamera;
	public Transform waypoint;
	private float zDepth;

	void Start ()
	{
		if (sourceCamera == null)
			sourceCamera = Camera.main;
		if (destCamera == null)
			destCamera = GetComponentInParent<Camera> ();

		zDepth = destCamera.transform.InverseTransformPoint (transform.position).z;
	}

	void LateUpdate ()
	{
		if (sourceCamera != null && destCamera != null && waypoint != null) {
			Vector3 waypointScreenPosition = sourceCamera.WorldToScreenPoint (waypoint.position);

			if (waypointScreenPosition.z > 0) {
				waypointScreenPosition.z = zDepth;
				transform.position = destCamera.ScreenToWorldPoint (waypointScreenPosition);
			} else 
				transform.position = destCamera.ScreenToWorldPoint (waypointScreenPosition);
		}
	}
}
