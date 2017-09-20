using UnityEngine;
using System.Collections;

public abstract class NumberTextDisplayer : NumberDisplayer
{
	private float numberValue = 0;
	private TextDisplayer textDisplayer;

	public float NumberValue {
		get { return numberValue;}
		set {
			Display (value);
			StopAllCoroutines ();
		}
	}

	public TextDisplayer TextDisplayer {
		get {
			if (textDisplayer == null) {
				textDisplayer = GetComponent<TextDisplayer> ();
				if (textDisplayer == null)
					textDisplayer = gameObject.AddComponent<GenericTextDisplayer> ();
			}
			return textDisplayer;
		}
	}

	protected override void Display (float value)
	{
		numberValue = value;
		TextDisplayer.Text = FormatNumberValueToString (numberValue);
	}
	
	protected abstract string FormatNumberValueToString (float numberValue);
	
	public void TweenValueTo (float targetValue, float time)
	{
		StopAllCoroutines ();
		StartCoroutine (DoTweenValueTo (targetValue, time));
	}
	
	private IEnumerator DoTweenValueTo (float targetValue, float time)
	{
		float timeElapsed = 0;
		float startValue = numberValue;
		while (timeElapsed < time) {
			yield return null;
			timeElapsed += Time.deltaTime;
			float newScore = Mathf.Lerp ((float)startValue, (float)targetValue, timeElapsed / time);
			Display (newScore);
		}
	}
}
