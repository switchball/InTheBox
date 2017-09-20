using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour
{
	void Start ()
	{
		GameObject.DontDestroyOnLoad (gameObject);
	}
}
