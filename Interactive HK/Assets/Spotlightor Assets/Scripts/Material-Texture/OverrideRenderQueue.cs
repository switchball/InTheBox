using UnityEngine;
using System.Collections;

// DEFAULT RENDER QUEUE
// Background	= 1000
// Geometry 	= 2000
// Transparent	= 3000
// Overlay		= 4000
public class OverrideRenderQueue : MonoBehaviour
{
	[Header ("Geometry = 2000, Transparent = 3000")]
	public int renderQueue = 3001;
	public bool useSharedMaterial = false;

	void Start ()
	{
		SetRenderQueue ();
	}

	[ContextMenu ("Set Render Queue")]
	public void SetRenderQueue ()
	{
		if (useSharedMaterial)
			GetComponent<Renderer> ().sharedMaterial.renderQueue = renderQueue;
		else
			GetComponent<Renderer> ().material.renderQueue = renderQueue;
	}
}
