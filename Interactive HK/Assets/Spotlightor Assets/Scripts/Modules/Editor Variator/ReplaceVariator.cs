#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System;

public class ReplaceVariator : Variator
{
	[System.Serializable]
	public class ReplaceVariation : Variation
	{
		public GameObject prefab;
		
		public override void Apply (GameObject target)
		{
#if UNITY_EDITOR
			if (target != prefab) {
				GameObject replacer = PrefabUtility.InstantiatePrefab (prefab) as GameObject;

				replacer.transform.SetParent (target.transform.parent);
				replacer.transform.CopyPositionRotation (target.transform);
				replacer.transform.localScale = target.transform.localScale;
				replacer.transform.SetSiblingIndex(target.transform.GetSiblingIndex());

				int selectionIndex = Array.IndexOf (Selection.gameObjects, target);
				if (selectionIndex >= 0){
					GameObject[] gos = Selection.gameObjects;
					gos[selectionIndex] = replacer;
					Selection.objects =gos;
				}

				DestroyImmediate (target);
			}
#endif
		}
	}
	
	public ReplaceVariation[] variations;
	
	public override Variation[] Variations { get { return variations; } }
}
