using UnityEngine;
using System.Collections;

public class RotateRandomVariator : Variator
{
	[System.Serializable]
	public class RotateRandomVariation : Variation
	{
		public Vector3 axis = Vector3.up;
		public RandomRangeFloat randomAngle = new RandomRangeFloat (-180, 180);
		public float angleStep = 15;

		public override void Apply (GameObject target)
		{
			float rotateAngle = randomAngle.RandomValue;
			rotateAngle = Mathf.RoundToInt (rotateAngle / angleStep) * angleStep;
			target.transform.Rotate (axis, rotateAngle, Space.Self);
		}
	}

	public RotateRandomVariation[] variations = new RotateRandomVariation[]{ new RotateRandomVariation () };

	public override Variation[] Variations { get { return variations; } }
}
