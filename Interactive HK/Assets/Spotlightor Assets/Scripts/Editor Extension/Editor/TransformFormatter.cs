using UnityEngine;
using UnityEditor;
using System.Collections;

public class TransformFormatter
{
	[MenuItem ("GameObject/Snap Position &p", false, 210)]
	static void SnapPosition ()
	{
		SnapPositionByUnit (1f);
	}

	[MenuItem ("GameObject/Snap Position 0.5 &#p", false, 210)]
	static void SnapPositionPoint5 ()
	{
		SnapPositionByUnit (0.5f);
	}

	static void SnapPositionByUnit (float unit)
	{
		Transform[] selectedTransforms = Selection.transforms;
		Undo.RecordObjects (selectedTransforms, "Snap local position");
		foreach (Transform transform in selectedTransforms) {
			Vector3 snappedPos = transform.localPosition;
			snappedPos.x = Mathf.RoundToInt (snappedPos.x / unit) * unit;
			snappedPos.y = Mathf.RoundToInt (snappedPos.y / unit) * unit;
			snappedPos.z = Mathf.RoundToInt (snappedPos.z / unit) * unit;
			transform.localPosition = snappedPos;
		}
	}

	[MenuItem ("GameObject/Snap Rotation &r", false, 211)]
	static void SnapRotation ()
	{
		Transform[] selectedTransforms = Selection.transforms;
		Undo.RecordObjects (selectedTransforms, "Snap local rotation");
		float snapStep = 15f;
		foreach (Transform transform in selectedTransforms) {
			Vector3 snappedRot = transform.localEulerAngles;
			snappedRot.x = Mathf.RoundToInt (snappedRot.x / snapStep) * snapStep;
			snappedRot.y = Mathf.RoundToInt (snappedRot.y / snapStep) * snapStep;
			snappedRot.z = Mathf.RoundToInt (snappedRot.z / snapStep) * snapStep;
			transform.localEulerAngles = snappedRot;
		}
	}

	[MenuItem ("GameObject/Snap Scale &s", false, 212)]
	static void SnapScale ()
	{
		Transform[] selectedTransforms = Selection.transforms;
		
		Undo.RecordObjects (selectedTransforms, "Snap local scale");
		foreach (Transform transform in selectedTransforms) {
			Vector3 snappedScale = transform.localScale;
			snappedScale.x = Mathf.RoundToInt (snappedScale.x);
			snappedScale.y = Mathf.RoundToInt (snappedScale.y);
			snappedScale.z = Mathf.RoundToInt (snappedScale.z);
			transform.localScale = snappedScale;
		}
	}

	[MenuItem ("GameObject/Zero Position &0", false, 213)]
	static void ZeroPosition ()
	{
		Transform[] selectedTransforms = Selection.transforms;
		
		Undo.RecordObjects (selectedTransforms, "Clear Local Transformation");
		foreach (Transform transform in selectedTransforms) {
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}
	}
}
