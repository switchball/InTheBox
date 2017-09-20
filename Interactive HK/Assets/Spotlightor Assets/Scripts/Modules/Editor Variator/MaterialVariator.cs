using UnityEngine;
using System.Collections;

public class MaterialVariator : Variator
{
	[System.Serializable]
	public class MaterialVariation : Variation
	{
		public Material material;

		public override void Apply (GameObject target)
		{
			MaterialVariator mv = target.GetComponent<MaterialVariator> ();
			Renderer[] rds;
			if (mv != null)
				rds = mv.renderers;
			else
				rds = target.GetComponentsInChildren<Renderer> ();
			if (rds != null) {
				foreach (Renderer rd in rds) {
					if (rd != null)
						rd.sharedMaterial = material;
				}
			}
		}
	}

	public Renderer[] renderers;
	public MaterialVariation[] variations;

	public override Variation[] Variations { get { return variations; } }
}
