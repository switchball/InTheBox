using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(TextDisplayer))]
public class TypeWriterTextDisplayer : MonoBehaviour
{
	public delegate void SimpleEventHandler (TypeWriterTextDisplayer displayer);

	public event SimpleEventHandler TypewritingStarted;
	public event SimpleEventHandler TypewritingCompleted;

	public float charsPerSecond = 30;
	public UnityEvent typewritingStarted;
	public UnityEvent typewritingCompleted;
	private TextDisplayer textDispalyer;
	private string typewrtingText = "";

	public bool IsTypewriting{ get; private set; }

	private string Text {
		set {
			if (textDispalyer == null) {
				textDispalyer = GetComponent<TextDisplayer> ();
				if (textDispalyer == null)
					textDispalyer = gameObject.AddComponent<GenericTextDisplayer> ();
			}
			textDispalyer.Text = value;
		}
	}

	public void TypewriteText (string text)
	{
		typewrtingText = text;
		StopCoroutine ("TypeWriteAllCharacters");
		OnStarted ();

		if (gameObject.activeInHierarchy && this.enabled)
			StartCoroutine ("TypeWriteAllCharacters");
		else
			CompleteTypewriting ();
	}

	public void CompleteTypewriting ()
	{
		StopCoroutine ("TypeWriteAllCharacters");
		Text = typewrtingText;

		OnCompleted ();
	}
	
	private IEnumerator TypeWriteAllCharacters ()
	{
		Text = "";
		string typeWriterText = "";
		for (int i = 0; i < typewrtingText.Length; i++) {
			yield return new WaitForSeconds (1f / charsPerSecond);
			typeWriterText += typewrtingText [i];
			Text = typeWriterText;
		}
		OnCompleted ();
	}

	private void OnStarted ()
	{
		IsTypewriting = true;

		typewritingStarted.Invoke ();
		if (TypewritingStarted != null)
			TypewritingStarted (this);
	}

	private void OnCompleted ()
	{
		IsTypewriting = false;

		typewritingCompleted.Invoke ();
		if (TypewritingCompleted != null)
			TypewritingCompleted (this);
	}
}
