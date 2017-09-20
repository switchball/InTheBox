using UnityEngine;
using System.Collections;

public class RangeInt
{
	public delegate void BasicEventHandler (RangeInt source);

	public delegate void ValueChangedEventHandler (RangeInt source,int amount);
	
	public event ValueChangedEventHandler ValueChanged;
	public event BasicEventHandler MinValueReached;
	public event BasicEventHandler MaxValueReached;
	
	private int minValue;
	private int maxValue;
	private int currentValue;

	public int MaxValue {
		get { return maxValue; }
		set {
			maxValue = value;
			minValue = Mathf.Min (minValue, maxValue - 1);
			Value = Mathf.Clamp (Value, minValue, maxValue);
		}
	}

	public int MinValue {
		get { return minValue; }
		set {
			minValue = value;
			maxValue = Mathf.Max (maxValue, minValue + 1);
			Value = Mathf.Clamp (Value, minValue, maxValue);
		}
	}

	public int Value {
		get { return currentValue; }
		set {
			if (value == currentValue)
				return;
			
			int oldValue = currentValue;
			currentValue = Mathf.Clamp (value, minValue, maxValue);
			if (currentValue != oldValue) {
				if (ValueChanged != null)
					ValueChanged (this, currentValue - oldValue);
				
				if (currentValue == maxValue && MaxValueReached != null)
					MaxValueReached (this);
				else if (currentValue == minValue && MinValueReached != null)
					MinValueReached (this);
			}
		}
	}

	public float Progress{ get { return Mathf.InverseLerp (minValue, maxValue, currentValue); } }

	public RangeInt (int min, int max, int defaultValue)
	{
		this.MinValue = min;
		this.MaxValue = max;
		this.Value = defaultValue;
	}

	public static implicit operator int (RangeInt safeInt)
	{
		return safeInt.Value;
	}
}
