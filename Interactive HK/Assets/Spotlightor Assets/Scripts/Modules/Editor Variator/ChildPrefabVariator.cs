#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System;

public class ChildPrefabVariator : Variator
{
	[System.Serializable]
	public class ChildPrefabVariation : Variation
	{
		public GameObject childPrefab;
		public bool exclusive = true;
		
		public override void Apply (GameObject target)
		{
			#if UNITY_EDITOR
			if (exclusive) {
				int i = 0;
				ChildPrefabVariator variatorInstance = target.GetComponent<ChildPrefabVariator> ();
				while (i < target.transform.childCount) {
					GameObject childGo = target.transform.GetChild (i).gameObject;
					if (variatorInstance.IsInstanceFromVariations (childGo) && PrefabUtility.GetPrefabParent (childGo) != childPrefab) {
						Undo.DestroyObjectImmediate (childGo);
					} else
						i++;
				}
			}
			for (int j = 0; j < target.transform.childCount; j++) {
				GameObject childGo = target.transform.GetChild (j).gameObject;
				if (PrefabUtility.GetPrefabParent (childGo) == childPrefab) {
					Undo.DestroyObjectImmediate (childGo);
					return;
				}
			}

			if (childPrefab != null) {
				GameObject childInstance = PrefabUtility.InstantiatePrefab (childPrefab) as GameObject;
				
				childInstance.transform.SetParent (target.transform, false);
				childInstance.transform.localPosition = childPrefab.transform.localPosition;
				childInstance.transform.localRotation = childPrefab.transform.localRotation;
				childInstance.transform.localScale = childPrefab.transform.localScale;

				Undo.RegisterCreatedObjectUndo(childInstance, "Instantiate "+childInstance.name);
			}
			#endif
		}
	}
	
	public ChildPrefabVariation[] variations;
	
	public override Variation[] Variations { get { return variations; } }

	private bool IsInstanceFromVariations (GameObject go)
	{
		bool result = false;
#if UNITY_EDITOR
		foreach (ChildPrefabVariation variation in variations) {
			if (variation.childPrefab != null && PrefabUtility.GetPrefabParent (go) == variation.childPrefab) {
				result = true;
				break;
			}
		}
#endif
		return result;
	}
}
