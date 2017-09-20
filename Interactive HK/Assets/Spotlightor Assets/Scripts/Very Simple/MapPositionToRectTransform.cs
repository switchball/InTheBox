using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class MapPositionToRectTransform : MonoBehaviour
{
	private static Vector2 initialAnchoredPosition = new Vector2 (999999, 999999);
	public Camera sourceCamera;
	public Transform waypoint;

	IEnumerator Start ()
	{
		if (waypoint != null) {
			Canvas canvas = transform.GetComponentInParent<Canvas> ();
			if (canvas != null) {
				GetComponent<RectTransform> ().anchoredPosition = initialAnchoredPosition;

				while (canvas.worldCamera == null)
					yield return null;

				yield return null; // Wait for canvas update

				MapPositionBetweenCameras positionMapper = gameObject.AddComponent<MapPositionBetweenCameras> ();
				positionMapper.sourceCamera = sourceCamera;
				positionMapper.destCamera = canvas.worldCamera;
				positionMapper.waypoint = waypoint;
			} else
				this.LogWarning ("Need a canvas in parent to work!");
		}
	}
}
