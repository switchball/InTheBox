using UnityEngine;
using System.Collections;

public class RotateVariator : Variator
{
	[System.Serializable]
	public class RotateVariation : Variation
	{
		public Vector3 rotation = new Vector3 (0, 90, 0);

		public override void Apply (GameObject target)
		{
			target.transform.Rotate(rotation, Space.Self);
		}
	}

	public RotateVariation[] variations;

	public override Variation[] Variations { get { return variations; } }
}
