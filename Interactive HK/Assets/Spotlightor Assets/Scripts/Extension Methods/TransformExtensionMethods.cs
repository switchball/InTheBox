using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TransformExtensionMethods
{

	public static void SetPositionX (this Transform transform, float x)
	{
		transform.position = new Vector3 (x, transform.position.y, transform.position.z);
	}

	public static void SetPositionY (this Transform transform, float y)
	{
		transform.position = new Vector3 (transform.position.x, y, transform.position.z);
	}

	public static void SetPositionZ (this Transform transform, float z)
	{
		transform.position = new Vector3 (transform.position.x, transform.position.y, z);
	}

	public static void SetLocalPositionX (this Transform transform, float x)
	{
		transform.localPosition = new Vector3 (x, transform.localPosition.y, transform.localPosition.z);
	}

	public static void SetLocalPositionY (this Transform transform, float y)
	{
		transform.localPosition = new Vector3 (transform.localPosition.x, y, transform.localPosition.z);
	}

	public static void SetLocalPositionZ (this Transform transform, float z)
	{
		transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, z);
	}

	public static void SetLocalEulerAngleX (this Transform transform, float x)
	{
		transform.localEulerAngles = new Vector3 (x, transform.localEulerAngles.y, transform.localEulerAngles.z);
	}

	public static void SetLocalEulerAngleY (this Transform transform, float y)
	{
		transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, y, transform.localEulerAngles.z);
	}

	public static void SetLocalEulerAngleZ (this Transform transform, float z)
	{
		transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, transform.localEulerAngles.y, z);
	}

	public static void SetLocalScaleX (this Transform transform, float x)
	{
		transform.localScale = new Vector3 (x, transform.localScale.y, transform.localScale.z);
	}

	public static void SetLocalScaleY (this Transform transform, float y)
	{
		transform.localScale = new Vector3 (transform.localScale.x, y, transform.localScale.z);
	}

	public static void SetLocalScaleZ (this Transform transform, float z)
	{
		transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, z);
	}

	public static void SetUniformLocalScale (this Transform transform, float uniformScale)
	{
		transform.localScale = Vector3.one * uniformScale;
	}

	public static void CopyPositionRotation (this Transform transform, Transform targetTransform)
	{
		transform.position = targetTransform.position;
		transform.rotation = targetTransform.rotation;
	}

	public static Transform FindParent (this Transform transform, string parentName)
	{
		Transform result = transform.parent;
		while (result != null && result.name != parentName)
			result = result.parent;

		return result;
	}

	public static T FindComponentInParent<T> (this Transform transform) where T: Component
	{
		T result = null;
		Transform currentTransform = transform;
		do {
			result = currentTransform.GetComponent<T> ();
			currentTransform = currentTransform.parent;
		} while(result == null && currentTransform != null);

		return result;
	}

	public static Transform FindChildDeep (this Transform transform, string childName)
	{
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild (i);
			if (child.name == childName)
				return child;
			else {
				Transform resultDeep = child.FindChildDeep (childName);
				if (resultDeep != null)
					return resultDeep;
			}
		}
		return null;
	}

	public static List<Transform> FindChildrenDeep (this Transform transform, string childName)
	{
		List<Transform> result = new List<Transform> ();
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild (i);
			if (child.name == childName)
				result.Add (child);
			if (child.childCount > 0)
				result.AddRange (child.FindChildrenDeep (childName));
		}
		return result;
	}

	public static float DistanceTo (this Transform transform, Transform target)
	{
		return Vector3.Distance (transform.position, target.position);
	}

	public static float DistanceTo (this Transform transform, GameObject target)
	{
		return Vector3.Distance (transform.position, target.transform.position);
	}

	public static float DistanceTo (this Transform transform, Component target)
	{
		return Vector3.Distance (transform.position, target.transform.position);
	}

	public static float DistanceTo (this Transform transform, Vector3 position)
	{
		return Vector3.Distance (transform.position, position);
	}

	public static Quaternion WorldToLocalRotationInParent (this Transform transform, Quaternion worldRotation)
	{
		return Quaternion.Inverse (transform.parent.rotation) * worldRotation;
	}
}
