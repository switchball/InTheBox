using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(TextDisplayer))]
public class TypeWriterParagraphDisplayer : MonoBehaviour
{
	public delegate void SimpleEventHandler (TypeWriterParagraphDisplayer displayer);

	public event SimpleEventHandler TypewritingStarted;
	public event SimpleEventHandler TypewritingCompleted;

	public float charsPerSecond = 30;
    public float indicatorSpeed = 5;
    public string[] textTargets;
	public UnityEvent typewritingStarted;
	public UnityEvent typewritingCompleted;
	private TextDisplayer textDispalyer;
    private string[] typewritingParagraph;
    private string typewritingText;
    private float mDelay;
    private AudioSource audioSource;

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

    public void TypewriteParagraph (float delay)
    {
        TypewriteParagraph(textTargets, delay);
    }

	public void TypewriteParagraph (string[] texts, float delay)
	{
        mDelay = delay;
		typewritingParagraph = texts;
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
		Text = typewritingText;

		OnCompleted ();
	}
	
	private IEnumerator TypeWriteAllCharacters ()
	{
        int indicatorFlag = 0;
		Text = "";
        yield return new WaitForSeconds(mDelay);
        foreach(string textLine in typewritingParagraph)
        {
            audioSource.Play();
            Text = "";
            typewritingText = textLine; // set target text
		    string typeWriterText = "";
            float accTime = 0; int i = 0;
            while (i < textLine.Length)
            {
                yield return new WaitForSeconds(1f / indicatorSpeed);
                accTime += 1f / indicatorSpeed;
                if (accTime + 1e-6 >= 1f / charsPerSecond)
                {
                    typeWriterText += textLine[i];
                    accTime -= 1f / charsPerSecond;
                    i++;
                }
                Text = typeWriterText + (indicatorFlag++ % 2 == 0 ? "|" : "");
            }
            audioSource.Stop();
            for (int j = 0; j < indicatorSpeed; j++)
            {
                Text = typewritingText + (indicatorFlag++ % 2 == 0 ? "|" : "");
                yield return new WaitForSeconds(1f / indicatorSpeed);
            }
        }
        Text = typewritingText;
        OnCompleted ();
	}

	private void OnStarted ()
	{
        // set audio source
        if (audioSource == null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
		IsTypewriting = true;

		typewritingStarted.Invoke ();
		if (TypewritingStarted != null)
			TypewritingStarted (this);
	}

	private void OnCompleted ()
	{
        audioSource.Stop();
		IsTypewriting = false;

		typewritingCompleted.Invoke ();
		if (TypewritingCompleted != null)
			TypewritingCompleted (this);
	}

    private void Reset()
    {
        var text = gameObject.GetComponent<Text>();
        if (text)
        {
            textTargets = text.text.Split('\n');
        }
    }
}
