using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Terrain))]
public class EditorDynamicTerrainData : MonoBehaviour
{
	public int detailResolutionPerPatch = 8;
	#if UNITY_EDITOR
	private static Dictionary<string, TerrainData> dynamicTerrainDataDict = new Dictionary<string, TerrainData> ();
	#endif

	void Awake ()
	{
		#if UNITY_EDITOR
		Terrain terrain = GetComponent<Terrain> ();
		TerrainData data = terrain.terrainData;

		if (dynamicTerrainDataDict.ContainsValue (data) == false) {
			TerrainData dynamicData = null;
			if (dynamicTerrainDataDict.TryGetValue (data.name, out dynamicData) == false) {
				dynamicData = new TerrainData ();

				dynamicData.name = "DYNAMIC " + data.name;
				
				dynamicData.alphamapResolution = data.alphamapResolution;
				dynamicData.baseMapResolution = data.baseMapResolution;
				dynamicData.detailPrototypes = data.detailPrototypes;
				dynamicData.heightmapResolution = data.heightmapResolution;
				dynamicData.size = data.size;
				dynamicData.splatPrototypes = data.splatPrototypes;
				dynamicData.treeInstances = data.treeInstances;
				dynamicData.treePrototypes = data.treePrototypes;
				dynamicData.wavingGrassAmount = data.wavingGrassAmount;
				dynamicData.wavingGrassSpeed = data.wavingGrassSpeed;
				dynamicData.wavingGrassStrength = data.wavingGrassStrength;
				dynamicData.wavingGrassTint = data.wavingGrassTint;
				
				dynamicData.SetAlphamaps (0, 0, data.GetAlphamaps (0, 0, data.alphamapWidth, data.alphamapHeight));
				dynamicData.SetHeights (0, 0, data.GetHeights (0, 0, data.heightmapWidth, data.heightmapHeight));

				dynamicData.SetDetailResolution (data.detailResolution, detailResolutionPerPatch);
				for (int detaiLayer = 0; detaiLayer< data.detailPrototypes.Length; detaiLayer++)
					dynamicData.SetDetailLayer (0, 0, detaiLayer, data.GetDetailLayer (0, 0, data.detailWidth, data.detailHeight, detaiLayer));
				dynamicData.RefreshPrototypes ();

				dynamicTerrainDataDict.Add (data.name, dynamicData);
			}

			terrain.terrainData = dynamicData;
			terrain.Flush ();
		}
		#endif
		Destroy (this);
	}
}
