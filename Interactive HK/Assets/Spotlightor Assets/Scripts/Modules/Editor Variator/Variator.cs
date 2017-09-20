using UnityEngine;
using System.Collections;

public abstract class Variator : MonoBehaviour
{
	[System.Serializable]
	public abstract class Variation
	{
		public string name = "A";

		public abstract void Apply (GameObject target);
	}

	public abstract Variation[] Variations{ get; }
	
	void Awake ()
	{
		Destroy (this);
	}

	[ContextMenu("随机此项变化")]
	public void RandomVariation ()
	{
		if (Variations.Length > 0) {
			Variator.Variation variation = Variations [Random.Range (0, Variations.Length)];
			variation.Apply (gameObject);
		}
	}

	[ContextMenu("随机<所有>变化")]
	public void RandomAllVariators ()
	{
		Variator[] variators = GetComponents<Variator> ();
		foreach (Variator v in variators) 
			v.RandomVariation ();
	}

}
