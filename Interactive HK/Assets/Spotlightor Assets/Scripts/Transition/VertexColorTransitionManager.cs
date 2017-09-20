using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class VertexColorTransitionManager : ValueTransitionManager
{
	public Color colorIn = Color.white;
	public Color colorOut = new Color (1, 1, 1, 0);

	protected override void OnProgressValueUpdated (float progress)
	{
		MeshUtility.Tint (GetComponent<MeshFilter> ().mesh, Color.Lerp (colorOut, colorIn, progress));
	}

}
