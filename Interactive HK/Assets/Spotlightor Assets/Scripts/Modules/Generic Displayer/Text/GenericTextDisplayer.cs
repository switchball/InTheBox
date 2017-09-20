using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GenericTextDisplayer : TextDisplayer
{
	private delegate void DisplayTextDelegate (string text);

	private DisplayTextDelegate setText;

	protected override void Display (string text)
	{
		DisplayText (text);
	}

	private DisplayTextDelegate DisplayText {
		get {
			if (setText == null) {
				if (GetComponent<Text> () != null)
					setText = SetUiTextText;
				else if (GetComponent<TextMesh> () != null)
					setText = SetTextMeshText;
				else if (GetComponent<GUIText>() != null)
					setText = SetGuiTextText;
			}
			return setText;
		}
	}

	private void SetUiTextText (string text)
	{
		GetComponent<Text> ().text = text;
	}

	private void SetTextMeshText (string text)
	{
		GetComponent<TextMesh> ().text = text;
	}

	private void SetGuiTextText (string text)
	{
		GetComponent<GUIText>().text = text;
	}
}
