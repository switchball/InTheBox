using UnityEngine;
using System.Collections;

public static class TextMeshAutoWrapper
{
	public static void WrapInEuropeanStyle (TextMesh textMesh, float maxWidth)
	{
		string wrappedText = textMesh.text;
		int charIndex = 0;
		int lastWordSpaceEndingIndex = 0;
		float currentLineWidth = 0.0f;
		float lineWidthOfLastWord = 0;
		float maxWidthWithoutCharacterSize = 10 * maxWidth / textMesh.characterSize;
		while (charIndex < wrappedText.Length) {
			char character = wrappedText [charIndex];
			if (character == '\n' || character == '\r') {
				currentLineWidth = 0;
				lineWidthOfLastWord = 0;
			} else {
				CharacterInfo characterInfo;
				
				float characterWidth = 0;
				if (textMesh.font.GetCharacterInfo (character, out characterInfo)) {
#if UNITY_5
					characterWidth = characterInfo.advance;
#endif
					#if !UNITY_5
					characterWidth = characterInfo.width;
					#endif
				} else
					Debug.LogWarning (string.Format ("Unrecognized character encountered: {0}({1})", character, (int)character));
				
				currentLineWidth += characterWidth;
				
				if (character == ' ') {
					lastWordSpaceEndingIndex = charIndex;
					lineWidthOfLastWord = currentLineWidth;
				}
				
				if (currentLineWidth >= maxWidthWithoutCharacterSize) {
					wrappedText = wrappedText.Remove (lastWordSpaceEndingIndex, 1);
					wrappedText = wrappedText.Insert (lastWordSpaceEndingIndex, "\n");
					
					currentLineWidth = currentLineWidth - lineWidthOfLastWord;
				}
			}
			
			charIndex++;
		}
		
		textMesh.text = wrappedText;
	}

	public static void WrapInChineseStyle(TextMesh textMesh, float maxWidth)
	{
		string wrappedText = textMesh.text;
		int charIndex = 0;
		float currentLineWidth = 0.0f;
		float maxWidthWithoutCharacterSize = 10 * maxWidth / textMesh.characterSize;
		while (charIndex < wrappedText.Length) {
			char character = wrappedText [charIndex];
			if (character == '\n' || character == '\r') {
				currentLineWidth = 0;
			} else {
				CharacterInfo characterInfo;
				
				float characterWidth = 0;
				if (textMesh.font.GetCharacterInfo (character, out characterInfo)) {
#if UNITY_5
					characterWidth = characterInfo.advance;
#endif
					#if !UNITY_5
					characterWidth = characterInfo.width;
					#endif
				} else
					Debug.LogWarning (string.Format ("Unrecognized character encountered: {0}({1})", character, (int)character));
				
				currentLineWidth += characterWidth;
				
				if (currentLineWidth >= maxWidthWithoutCharacterSize) {
					wrappedText = wrappedText.Insert (charIndex, "\n");
					
					currentLineWidth = 0;
				}
			}
			
			charIndex++;
		}
		
		textMesh.text = wrappedText;
	}
}
