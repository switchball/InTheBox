using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SerializableEnumKeyDictionary<T, U> : SerializableDictionary<T,U> where T : struct, IConvertible
{
//	public override void OnBeforeSerialize ()
//	{
//		if (Application.isEditor && Application.isPlaying == false) {
//			if (keys != null) {
//				if (typeof(T).IsEnum)
//					SetKeysByEnumValues ();
//				else
//					Debug.LogError ("T must be Enum type!");
//			}
//		}
//
//		base.OnBeforeSerialize ();
//	}
//
//	private void SetKeysByEnumValues ()
//	{
//		Array enumValues = Enum.GetValues (typeof(T));
//		if (keys.Count > enumValues.Length)
//			keys.RemoveRange (enumValues.Length, keys.Count - enumValues.Length);
//		
//		for (int i = 0; i < keys.Count; i++)
//			keys [i] = (T)enumValues.GetValue (i);
//	}
}
