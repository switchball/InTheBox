using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameObjectExtensionMethods
{
	public static T GetComponentOfType<T> (this GameObject obj) where T:class
	{
		MonoBehaviour[] allMonoBehaviours = obj.GetComponents<MonoBehaviour> ();
		foreach (MonoBehaviour monoBehaviour in allMonoBehaviours) {
			if (monoBehaviour is T)
				return monoBehaviour as T;
		}
		return null;
	}
	
	public static T[] GetComponentsOfType<T> (this GameObject obj) where T:class
	{
		MonoBehaviour[] allMonoBehaviours = obj.GetComponents<MonoBehaviour> ();
		List<T> result = new List<T> ();
		foreach (MonoBehaviour monoBehaviour in allMonoBehaviours) {
			if (monoBehaviour is T) 
				result.Add (monoBehaviour as T);
		}
		return result.ToArray ();
	}
}
