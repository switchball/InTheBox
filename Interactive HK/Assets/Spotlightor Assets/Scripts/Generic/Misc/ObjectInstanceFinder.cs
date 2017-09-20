using UnityEngine;
using System.Collections;

public class ObjectInstanceFinder<T> where T : Behaviour
{
	private T instance;
	private string instanceTag = "";

	public T Instance {
		get {
			if (instance == null) {
				if (!string.IsNullOrEmpty (instanceTag)) {
					GameObject tagGo = GameObject.FindGameObjectWithTag (instanceTag);
					if (tagGo != null)
						instance = tagGo.GetComponent<T> ();
				}
				if (instance == null)
					instance = GameObject.FindObjectOfType (typeof(T)) as T;
			}
			return instance;
		}
	}

	public ObjectInstanceFinder ()
	{

	}

	public ObjectInstanceFinder (string tag)
	{
		this.instanceTag = tag;
	}
}