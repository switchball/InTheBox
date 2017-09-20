using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UiMultiStates))]
public class UiMultiStatesObjectActivator : MonoBehaviour
{
	public List<GameObject> objectByStateIds;

	// Use this for initialization
	void Start ()
	{
		UpdateObjectActivation (GetComponent<UiMultiStates> ());

		GetComponent<UiMultiStates> ().StateChanged += HandleStateChanged;
	}

	void HandleStateChanged (UiMultiStates multiStates)
	{
		UpdateObjectActivation (multiStates);
	}
	
	private void UpdateObjectActivation (UiMultiStates multiStates)
	{
		for (int i = 0; i < objectByStateIds.Count; i++)
			objectByStateIds [i].SetActive (i == multiStates.StateId);
	}
}
