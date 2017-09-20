using UnityEngine;
using System.Collections;

public class ChangeSaveEngine : MonoBehaviour
{
	public SaveEngine saveEngine;

	void Start ()
	{
		if (HybridSaveEngine.Instance.ActiveSaveEngine != saveEngine)
			HybridSaveEngine.Instance.ActiveSaveEngine = saveEngine;
	}
}
