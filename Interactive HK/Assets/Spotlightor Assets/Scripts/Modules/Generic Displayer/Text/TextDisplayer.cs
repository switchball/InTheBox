using UnityEngine;
using System.Collections;

public abstract class TextDisplayer : MonoBehaviour
{
	public delegate void TextUpdatedEventHandler (string text);
	
	public delegate void TextChangedEventHandler (string text);
	
	public event TextUpdatedEventHandler TextUpdated;
	public event TextChangedEventHandler TextChanged;

	private string text = "";
	
	public string Text {
		get { return text; }
		set {
			string oldValue = this.text;

			Display (value);
			this.text = value;
			
			if (TextUpdated != null)
				TextUpdated (value);
			if (TextChanged != null && oldValue != value) 
				TextChanged (value);
		}
	}

	protected abstract void Display (string text);
}