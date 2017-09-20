using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextureDisplayer : MonoBehaviour
{
	private delegate void SetTextureDelegate (Texture texture);

	private SetTextureDelegate setTexture;
	private Texture texture;

	public Texture Texture {
		get { return texture; }
		private set {
			texture = value;
			SetTexture (value);
		}
	}

	private SetTextureDelegate SetTexture {
		get {
			if (setTexture == null) {
				if (GetComponent<RawImage> () != null)
					setTexture = SetRawImageTexture;
				else if (GetComponent<Renderer>() != null)
					setTexture = SetRendererMaterialTexture;
			}
			return setTexture;
		}
	}

	private void SetRawImageTexture (Texture texture)
	{
		GetComponent<RawImage> ().texture = texture;
	}

	private void SetRendererMaterialTexture (Texture texture)
	{
		GetComponent<Renderer>().material.mainTexture = texture;
	}

	public virtual void Display (Texture imageTexture)
	{
		this.Texture = imageTexture;
	}
}
