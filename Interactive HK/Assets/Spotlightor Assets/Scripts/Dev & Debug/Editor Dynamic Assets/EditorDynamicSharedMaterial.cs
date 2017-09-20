using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class EditorDynamicSharedMaterial : MonoBehaviour
{
	public int materialIndex = 0;

	void Awake ()
	{
		#if UNITY_EDITOR
		if(GetComponent<Renderer>() != null && GetComponent<Renderer>().sharedMaterials.Length > materialIndex){
			Material[] sharedMaterials = GetComponent<Renderer>().sharedMaterials;
			sharedMaterials[materialIndex] = EditorDynamicMaterialsStorage.GetDynamicMaterial(sharedMaterials[materialIndex]);
			GetComponent<Renderer>().sharedMaterials = sharedMaterials;
			Destroy(this);
		}
		#endif
		Destroy (this);
	}
}
