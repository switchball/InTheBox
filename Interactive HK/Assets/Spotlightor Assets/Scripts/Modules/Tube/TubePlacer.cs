using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
public class TubePlacer : MonoBehaviour
{
	[System.Serializable]
	public class PartSetting
	{
		public List<TubePart> prefabs;

		public TubePart DefaultPrefab{ get { return prefabs.Count > 0 ? prefabs [0] : null; } }
	}
	public PartSetting head;
	public PartSetting tail;
	public PartSetting straight;
	public List<PartSetting> bendRight;
	public List<PartSetting> bendLeft;
	public List<PartSetting> bendUp;
	public List<PartSetting> bendDown;
	public float buttonSize = 2;
	public float bendArrowOffset = 2;

	void Update ()
	{
		if (Application.isEditor && !Application.isPlaying)
			UpdatePartsPlacement ();
	}

	public virtual void UpdatePartsPlacement ()
	{
		TubePart[] childParts = GetComponentsInChildren<TubePart> ();
		for (int i = 0; i < childParts.Length; i++) {
			TubePart part = childParts [i];
			if (i == 0) {
				part.transform.localPosition = Vector3.zero;
				part.transform.localRotation = Quaternion.identity;
			} else {
				TubePart prevPart = childParts [i - 1];
				part.transform.position = prevPart.EndWorldPosition;
				part.transform.rotation = prevPart.EndWorldRotation;
			}
		}
	}
}
