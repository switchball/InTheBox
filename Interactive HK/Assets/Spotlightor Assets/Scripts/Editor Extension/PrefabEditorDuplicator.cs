using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabEditorDuplicator : MonoBehaviour
{
	[System.Serializable]
	public class DuplicateWaypoint
	{
		public Vector3 position = Vector3.forward;
		public Vector3 rotation = Vector3.zero;
	}

	public List<DuplicateWaypoint> duplicateWaypoints;
	public float buttonSize = 1;
	public Vector3 buttonOffset = Vector3.zero;

	void Start ()
	{
		Destroy (this);
	}
}
