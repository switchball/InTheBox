using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EditorDynamicMaterialsStorage
{
	#if UNITY_EDITOR
	private static Dictionary<Material, Material> dynamicMaterialDictionary = new Dictionary<Material, Material> ();
	#endif

	public static Material GetDynamicMaterial (Material material)
	{
		#if UNITY_EDITOR
		Material replacementMaterial = null;
		if (dynamicMaterialDictionary.ContainsValue (material))
			replacementMaterial = material;
		else {
			if (dynamicMaterialDictionary.TryGetValue (material, out replacementMaterial) == false) {
				replacementMaterial = new Material (material);
				replacementMaterial.name = "RUNTIME " + material.name;

				dynamicMaterialDictionary [material] = replacementMaterial;
			}
		}
		return replacementMaterial;
		#endif
		#if !UNITY_EDITOR
		return material;
		#endif
	}
}
