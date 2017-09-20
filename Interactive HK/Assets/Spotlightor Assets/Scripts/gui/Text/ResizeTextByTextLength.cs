using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextDisplayer))]
[RequireComponent(typeof(TextMesh))]
public class ResizeTextByTextLength : FunctionalMonoBehaviour
{
	[System.Serializable]
	public class CharacterSizeSetting
	{
		public int textLength = 1;
		public float characterSize = 0.01f;
		public int fontSize = 0;
	}
	public CharacterSizeSetting[] sizesDecreaseByLength;
	#region implemented abstract members of FunctionalMonoBehaviour
	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		GetComponent<TextDisplayer> ().TextUpdated += HandleTextUpdated;
		HandleTextUpdated (GetComponent<TextDisplayer> ().Text);
	}

	protected override void OnBecameUnFunctional ()
	{
		GetComponent<TextDisplayer> ().TextUpdated -= HandleTextUpdated;
	}
	#endregion
	
	void HandleTextUpdated (string text)
	{
		if (sizesDecreaseByLength.Length > 0) {
			CharacterSizeSetting selectedSizeSetting = null;
			for (int i = 0; i < sizesDecreaseByLength.Length; i++) {
				CharacterSizeSetting setting = sizesDecreaseByLength [i];
				if (text.Length <= setting.textLength) {
					selectedSizeSetting = setting;
					break;
				}
			}
			if (selectedSizeSetting == null)
				selectedSizeSetting = sizesDecreaseByLength [sizesDecreaseByLength.Length - 1];
			TextMesh tm = GetComponent<TextMesh> ();
			tm.characterSize = selectedSizeSetting.characterSize;
			tm.fontSize = selectedSizeSetting.fontSize;
		}
	}
}
