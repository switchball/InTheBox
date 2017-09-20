using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class TextureFileSaver : MonoBehaviour
{
	public delegate void GenericEventHanlder (TextureFileSaver source);

	public event GenericEventHanlder Completed;

	public Texture2D texture;
	public string filePath;
	public int quality = 75;

	IEnumerator Start ()
	{
		if (texture != null && !string.IsNullOrEmpty (filePath)) {
			#if !UNITY_WEBPLAYER
			string extension = Path.GetExtension (filePath).Substring (1);
			Byte[] fileBytes;
			if (extension.ToLower () == "jpg")
				fileBytes = texture.EncodeToJPG (quality);
			else
				fileBytes = texture.EncodeToPNG ();

			File.WriteAllBytes (filePath, fileBytes);
			#endif
			#if UNITY_WEBPLAYER
			this.LogWarning("Cannot write bytes to file in WebPlayer!");
			#endif
			yield return 1;

			if (Completed != null) 
				Completed (this);

			Destroy (gameObject);
		} else
			Destroy (gameObject);
	}
}
