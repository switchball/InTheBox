#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System;

public class ChildActivationVariator : Variator
{
	[System.Serializable]
	public class ChildActivationVariation : Variation
	{
		public GameObject child;

		public override void Apply (GameObject target)
		{
			#if UNITY_EDITOR
			Undo.RecordObject(child, "Child Activation Variation");
			child.SetActive (!child.activeSelf);
			#endif
		}
	}

	public ChildActivationVariation[] variations;

	public override Variation[] Variations { get { return variations; } }

}
