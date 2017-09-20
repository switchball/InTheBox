using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextDisplayer))]
public class StylizeTextDisplayerText : MonoBehaviour
{
	public enum CaseConversionTypes
	{
		None = 0,
		ToUpper = 1,
		ToLower = 2
	}
	public CaseConversionTypes caseConverstion = CaseConversionTypes.None;
	private TextMesh myTextMesh;

	public TextMesh MyTextMesh {
		get {
			if (myTextMesh == null)
				myTextMesh = GetComponent<TextMesh> ();
			return myTextMesh;
		}
	}
	
	void Awake ()
	{
		Stylize ();
		GetComponent<TextDisplayer> ().TextUpdated += HandleTextUpdated;
	}

	void HandleTextUpdated (string text)
	{
		StylizeText (text);
	}
	
	public void Stylize ()
	{
		if (MyTextMesh)
			StylizeText (MyTextMesh.text);
		else if (GetComponent<GUIText>())
			StylizeText (GetComponent<GUIText>().text);
	}
	
	private void StylizeText (string text)
	{
		string styledText = text;
		switch (caseConverstion) {
		case CaseConversionTypes.ToLower:
			styledText = styledText.ToLower ();
			break;
		case CaseConversionTypes.ToUpper:
			styledText = styledText.ToUpper ();
			break;
		}
		
		if (MyTextMesh)
			MyTextMesh.text = styledText;
		else if (GetComponent<GUIText>())
			GetComponent<GUIText>().text = styledText;
	}
}
