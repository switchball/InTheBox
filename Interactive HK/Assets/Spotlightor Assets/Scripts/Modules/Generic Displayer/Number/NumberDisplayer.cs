using UnityEngine;
using System.Collections;

public abstract class NumberDisplayer : MonoBehaviour
{
	private float value = 0;
	
	public float Value {
		get { return this.value; }
		set {
			this.value = value;
			Display (value);
		}
	}
	
	protected abstract void Display (float value);
}
