using UnityEngine;
using System.Collections;

public class GoFsmState : MonoBehaviour, IFsmState<GoFsmController,int>
{
	private GoFsmController owner;

	public int StateId { get { return this.GetInstanceID (); } }

	public GoFsmController Owner {
		get { return owner; }
		set { this.owner = value; }
	}

	public virtual void BeginState (int previousState)
	{
		gameObject.SetActive (true);
	}

	public virtual void EndState (int newState)
	{
		gameObject.SetActive (false);
	}
}