using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Terrain))]
public class EditorDynamicTerrainMaterialTemplate : MonoBehaviour
{
	void Awake ()
	{
		#if UNITY_EDITOR
		Terrain terrain = GetComponent<Terrain>();
		if(terrain != null && terrain.materialTemplate != null){
			terrain.materialTemplate = EditorDynamicMaterialsStorage.GetDynamicMaterial(terrain.materialTemplate);
			Destroy(this);
		}
		#endif
		Destroy (this);
	}
}
