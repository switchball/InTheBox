using UnityEngine;
using System.Collections;

public class MeshVariator : Variator
{
	[System.Serializable]
	public class MeshVariation : Variation
	{
		public Mesh mesh;

		public override void Apply (GameObject target)
		{
			MeshVariator targetMeshVariator = target.GetComponent<MeshVariator> ();
			MeshFilter targetMeshFileter;
			MeshCollider targetMeshCollider;
			if (targetMeshVariator != null) {
				targetMeshFileter = targetMeshVariator.targetMeshFilter;
				targetMeshCollider = targetMeshVariator.targetMeshCollider;
			} else {
				targetMeshFileter = target.GetComponent<MeshFilter> ();
				targetMeshCollider = target.GetComponent<MeshCollider> ();
			}
			if (targetMeshFileter != null)
				targetMeshFileter.sharedMesh = mesh;
			if (targetMeshCollider != null)
				targetMeshCollider.sharedMesh = mesh;
		}
	}

	public MeshFilter targetMeshFilter;
	public MeshCollider targetMeshCollider;
	public MeshVariation[] variations;

	public override Variation[] Variations { get { return variations; } }

	void Reset ()
	{
		if (targetMeshFilter == null)
			targetMeshFilter = GetComponent<MeshFilter> ();
		if (targetMeshCollider == null)
			targetMeshCollider = GetComponent<MeshCollider> ();
	}
}
