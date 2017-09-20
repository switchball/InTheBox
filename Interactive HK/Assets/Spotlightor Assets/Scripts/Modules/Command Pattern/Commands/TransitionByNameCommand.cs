using UnityEngine;
using System.Collections;

public class TransitionByNameCommand : TransitionCommand
{
	public string searchRootObjectName = "";
	public string transitionObjectName = "";

	protected override IEnumerator CoroutineCommand ()
	{
		GameObject transitionGo = null;
		if (string.IsNullOrEmpty (searchRootObjectName))
			transitionGo = GameObject.Find (transitionObjectName);
		else {
			GameObject searchRoot = GameObject.Find (searchRootObjectName);
			if (searchRoot != null) {
				Transform transitionTransform = searchRoot.transform.FindChildDeep (transitionObjectName);
				if (transitionTransform != null)
					transitionGo = transitionTransform.gameObject;
			}
		}
		if (transitionGo != null)
			this.transition = transitionGo.GetComponent<TransitionManager> ();
		
		return base.CoroutineCommand ();
	}
}
