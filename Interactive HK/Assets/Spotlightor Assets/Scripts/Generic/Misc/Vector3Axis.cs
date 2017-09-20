using UnityEngine;
using System.Collections;

[System.Serializable]
public class Vector3Axis
{
	public enum AxisTypes
	{
		X,
		Y,
		Z
	}

	public AxisTypes axis = AxisTypes.X;

	public float GetAxisValue (Vector3 vector)
	{
		if (axis == AxisTypes.X)
			return vector.x;
		else if (axis == AxisTypes.Y)
			return vector.y;
		else
			return vector.z;
	}
}
