using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh), typeof(TextDisplayer))]
public class WrapTextMeshByLanguage : MonoBehaviour
{
	public float maxWidth = 5;
	private TextMesh textMesh;

	protected Font TextFont {
		get { return MyTextMesh.font;}
	}

	protected TextMesh MyTextMesh {
		get {
			if (textMesh == null)
				textMesh = GetComponent<TextMesh> ();
			return textMesh;
		}
	}

	void Start ()
	{
		WrapText ();
		GetComponent<TextDisplayer> ().TextUpdated += HandleTextUpdated;
	}

	void HandleTextUpdated (string text)
	{
		WrapText ();
	}

	[ContextMenu("Wrap")]
	void WrapText ()
	{
		if (Localization.CurrentLanguage != LocalizationLanguageTypes.Chinese)
			TextMeshAutoWrapper.WrapInEuropeanStyle (MyTextMesh, maxWidth);
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.DrawLine (transform.position, transform.TransformPoint (Vector3.right * maxWidth));
	}
}
