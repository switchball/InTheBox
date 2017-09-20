using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiMultiStates : MonoBehaviour
{
	public delegate void StateChangedEventHandler (UiMultiStates uiMultiStates);
	
	public event StateChangedEventHandler StateChanged;
	
	public int statesCount = 2;
	private int stateId = 0;
	
	public int StateId {
		get { return stateId;}
		set {
			value = value % statesCount;
			if (stateId != value) {
				stateId = value;
				if (StateChanged != null)
					StateChanged (this);
			}
		}
	}
}
