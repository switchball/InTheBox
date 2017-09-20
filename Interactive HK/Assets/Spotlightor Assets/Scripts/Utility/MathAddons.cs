using UnityEngine;
using System.Collections;

/// <summary>
/// Addons of math functions
/// </summary>
public static class MathAddons
{
	public const float TWO_PI = Mathf.PI * 2;
	public const float HALF_PI = Mathf.PI * 0.5f;

	public static float LerpUncapped (float from, float to, float t)
	{
		return from + (to - from) * t;
	}

	public static Vector3 LerpUncapped (Vector3 from, Vector3 to, float t)
	{
		return from + (to - from) * t;
	}

	public static Vector2 LerpUncapped (Vector2 from, Vector2 to, float t)
	{
		return from + (to - from) * t;
	}

	/// <summary>
	/// Is value in range min to max
	/// </summary>
	/// <param name="value"></param>
	/// <param name="min"></param>
	/// <param name="max"></param>
	/// <returns></returns>
	public static bool IsInRange (float value, float min, float max)
	{
		return (value >= min) && (value <= max);
	}
	/// <summary>
	/// Is value in range min to max
	/// </summary>
	/// <param name="value"></param>
	/// <param name="min"></param>
	/// <param name="max"></param>
	/// <returns></returns>
	public static bool IsInRange (int value, int min, int max)
	{
		return (value >= min) && (value <= max);
	}
	
	/// <summary>
	/// Get the critical damping acceleration.
	/// Read more:http://en.wikipedia.org/wiki/Damping
	/// </summary>
	/// <returns>
	/// The critical damping acceleration.
	/// </returns>
	/// <param name='k'>
	/// Spring constant K.
	/// </param>
	/// <param name='dx'>
	/// Distance to ballance position.
	/// </param>
	/// <param name='velocity'>
	/// Velocity.
	/// </param>
	/// <param name='m'>
	/// Mass
	/// </param>
	public static float GetCriticalDampingAcceleration (float k, float dx, float velocity, float m)
	{
		float fSpring = -k * dx;
		float c = 2 * Mathf.Sqrt (k);
		float fDamping = - c * velocity;
		float fTotal = fSpring + fDamping;
		return fTotal / m;
	}
	
	// Simplified version without mass(m = 1)
	public static float GetCriticalDampingAcceleration (float k, float dx, float velocity)
	{
		return - 2 * Mathf.Sqrt (k) * velocity - k * dx;
	}

	/// <summary>
	/// Gets the underdamped acceleration.
	/// This function is an modified version of GetCriticalDampingAcceleration. I'm not sure if it's right in math, but it looks OK.
	/// Read more:http://en.wikipedia.org/wiki/Damping
	/// </summary>
	/// <returns>The underdamped acceleration.</returns>
	/// <param name="k">Spring constant K.</param>
	/// <param name="dx">Distance to ballance position.</param>
	/// <param name="velocity">Velocity.</param>
	/// <param name="underdampedIntensity">Underdamped intensity (0~1). 0 = Critical Damping, 1 = un-stoppable damping.</param>
	public static float GetUnderdampedAcceleration (float k, float dx, float velocity, float underdampedIntensity)
	{
		underdampedIntensity = Mathf.Clamp01 (underdampedIntensity);
		underdampedIntensity = 1f - underdampedIntensity;
		return - 2 * Mathf.Sqrt (k) * velocity * underdampedIntensity - k * dx;
	}
	
	public static void FormatAngleInDegree (ref float angleInDegree)
	{
		angleInDegree %= 360;
		if (angleInDegree > 180)
			angleInDegree -= 360;
		else if (angleInDegree < -180)
			angleInDegree += 360;
	}
	
	public static void FormatAngleInRadian (ref float angleInRadian)
	{
		angleInRadian %= TWO_PI;
		if (angleInRadian > Mathf.PI)
			angleInRadian -= TWO_PI;
		else if (angleInRadian < -Mathf.PI)
			angleInRadian += TWO_PI;
	}

	public static Quaternion AverageQuaternions (Quaternion[] quaternions)
	{
		if (quaternions.Length == 0)
			return Quaternion.Euler (0, 0, 0);
		Quaternion average = new Quaternion (0, 0, 0, 0);
		
		for (int i = 0; i < quaternions.Length; i++) {
			Quaternion otherQ = quaternions [i];
			if (Quaternion.Dot (average, otherQ) > 0f) {
				average.w += otherQ.w;
				average.x += otherQ.x;
				average.y += otherQ.y;
				average.z += otherQ.z;
			} else {
				average.w -= otherQ.w;
				average.x -= otherQ.x;
				average.y -= otherQ.y;
				average.z -= otherQ.z;
			}
		}
		float mag = Mathf.Sqrt (average.x * average.x + average.y * average.y + average.z * average.z + average.w * average.w);
		if (mag > 0.0001f) {
			average.x /= mag;
			average.y /= mag;
			average.z /= mag;
			average.w /= mag;
		} else
			average = quaternions [0];
		
		return average;
	}
}
