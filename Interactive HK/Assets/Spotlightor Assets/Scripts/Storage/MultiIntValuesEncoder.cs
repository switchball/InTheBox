using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiIntValuesEncoder
{
	private class RangedInt
	{
		private int value = 0;

		public int Min{ get; private set; }

		public int Max{ get; private set; }

		public int Value { 
			get{ return this.value;}
			set {
				this.value = Mathf.Clamp (value, Min, Max);
			} 
		}

		public int ZeroBasedValue { 
			get { return Value - Min; } 
			set { Value = Min + value;}
		}

		public int PossibleValuesCount{ get { return Max - Min + 1; } }

		public RangedInt (int min, int max, int value)
		{
			this.Max = max;
			this.Min = Mathf.Min (min, max - 1);
			this.Value = value;
		}
	}
	private List<RangedInt> rangedIntSequence = new List<RangedInt> ();

	public void AddRangedInt (int min, int max, int value)
	{
		rangedIntSequence.Add (new RangedInt (min, max, value));
	}

	public int this [int index] {
		get {
			if (MathAddons.IsInRange (index, 0, rangedIntSequence.Count - 1))
				return rangedIntSequence [index].Value;
			else
				return -1;
		}
		set { 
			if (MathAddons.IsInRange (index, 0, rangedIntSequence.Count - 1))
				rangedIntSequence [index].Value = value;
		}
	}

	public int EncodeToSingleInt ()
	{
		int encodedInt = 0;
		int offset = 1;
		foreach (RangedInt rangedInt in rangedIntSequence) {
			encodedInt += rangedInt.ZeroBasedValue * offset;
			offset *= rangedInt.PossibleValuesCount;
		}
		if (offset < 0)
			Debug.LogError ("MultiIntValuesEncoder max encoded int > int.Max");
		return encodedInt;
	}

	public void DecodeFromSingleInt (int encodedInt)
	{
		int divider = 1;
		rangedIntSequence.ForEach (rangedInt => divider *= rangedInt.PossibleValuesCount);
		
		for (int i = rangedIntSequence.Count-1; i >= 0; i--) {
			RangedInt rangedInt = rangedIntSequence [i];

			divider /= rangedInt.PossibleValuesCount;
			if (divider == 0)
				divider = 1;
			int decodedValue = encodedInt / divider;
			rangedInt.ZeroBasedValue = decodedValue;

			encodedInt -= decodedValue * divider;
		}
	}
}
