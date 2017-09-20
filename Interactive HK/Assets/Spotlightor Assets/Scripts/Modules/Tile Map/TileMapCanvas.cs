using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Spotlightor.TileMap
{
	public class TileMapCanvas : MonoBehaviour
	{
		[System.Serializable]
		public class TileWaypoint
		{
			public int x = 0;
			public int z = 0;

			public TileWaypoint (int x, int z)
			{
				this.x = x;
				this.z = z;
			}
		}

		[System.Serializable]
		public class TilePrefabSetting
		{
			public Transform top;
			public Vector3 topRotation = Vector3.zero;
			public Transform wall;
			public Vector3 wallRotation = Vector3.zero;
		}

		public Vector3 tileSize = new Vector3 (2, 2, 2);

		public List<TilePrefabSetting> centers;
		[Header ("Edge")]
		public List<TilePrefabSetting> edgesLeft;
		public List<TilePrefabSetting> edgesRight;
		public List<TilePrefabSetting> edgesFront;
		public List<TilePrefabSetting> edgesBack;
		[Header ("Corner")]
		public List<TilePrefabSetting> cornersBL;
		public List<TilePrefabSetting> cornersBR;
		public List<TilePrefabSetting> cornersFL;
		public List<TilePrefabSetting> cornersFR;
		[Header ("Negative Corner")]
		public List<TilePrefabSetting> negativeCornersBL;
		public List<TilePrefabSetting> negativeCornersBR;
		public List<TilePrefabSetting> negativeCornersFL;
		public List<TilePrefabSetting> negativeCornersFR;


		[HideInInspector ()]
		public TileMapData data;

		public TileMap tileMapPrefab;

		void OnDrawGizmos ()
		{
			DrawCornerGizmosPicker ();
		}

		private void DrawCornerGizmosPicker ()
		{
			List<Vector3> cornerPositions = new List<Vector3> ();
			cornerPositions.Add (new Vector3 (data.MinX * tileSize.x, 0, data.MaxZ * tileSize.z));
			cornerPositions.Add (new Vector3 (data.MaxX * tileSize.x, 0, data.MaxZ * tileSize.z));
			cornerPositions.Add (new Vector3 (data.MinX * tileSize.x, 0, data.MinZ * tileSize.z));
			cornerPositions.Add (new Vector3 (data.MaxX * tileSize.x, 0, data.MinZ * tileSize.z));

			Vector3 buttonSize = new Vector3 (tileSize.x, 0, tileSize.z);
			Vector3 buttonWorldOffset = Vector3.down * 0.1f;
			Gizmos.color = new Color (1, 1, 1, 0.1f);

			cornerPositions.ForEach (cp =>
			Gizmos.DrawCube (transform.TransformPoint (cp) + buttonWorldOffset, buttonSize));
		}

		private void DrawPlaneGizmosPicker ()
		{
			float buttonHeight = 0.2f;
			Vector3 size = new Vector3 (data.MaxX - data.MinX, 0, data.MaxZ - data.MinZ);
			size.Scale (tileSize);
			size.y = buttonHeight;
			Vector3 center = new Vector3 (data.MaxX + data.MinX, 0, data.MaxZ + data.MinZ) * 0.5f;
			center.Scale (tileSize);

			Gizmos.color = new Color (1, 1, 1, 0.1f);
			Gizmos.DrawCube (transform.TransformPoint (center + Vector3.up * -0.1f + Vector3.down * size.y * 0.5f), transform.TransformVector (size));
		}

		[ContextMenu ("Replace with TileMap")]
		private void ReplaceWithNewTileMap ()
		{
			#if UNITY_EDITOR
			if (tileMapPrefab != null) {
				List<Transform> legacyChildTiles = new List<Transform> ();
				for (int i = 0; i < transform.childCount; i++)
					legacyChildTiles.Add (transform.GetChild (i));
				
				TileMap tileMap = PrefabUtility.InstantiatePrefab (tileMapPrefab) as TileMap;
				tileMap.transform.SetParent (transform.parent);
				tileMap.transform.CopyPositionRotation (transform);

				foreach (TileMapData.Tile tileData in data.tiles) {
					Vector3 localPos = new Vector3 (tileData.x, 0, tileData.z);
					localPos.Scale (tileMap.tileSize);

					Tile tile = PrefabUtility.InstantiatePrefab (tileMap.brushTile) as Tile;
					tile.transform.SetParent (tileMap.transform, false);
					tile.transform.localPosition = localPos;
					tile.transform.localRotation = Quaternion.identity;

					tile.WallCount = data.height;
				}

				tileMap.FormatTiles ();

				List<Tile> newChildTiles = new List<Tile> (tileMap.GetComponentsInChildren<Tile> ());
				foreach (Transform legacyTile in legacyChildTiles) {
					List<Transform> decos = new List<Transform> ();
					for (int i = 0; i < legacyTile.childCount; i++) {
						Transform child = legacyTile.GetChild (i);
						if (PrefabUtility.GetPrefabObject (child) != PrefabUtility.GetPrefabObject (legacyTile))
							decos.Add (child);
					}
					if (decos.Count > 0) {
						Tile nearestTile = null;
						foreach (Tile newTile in newChildTiles) {
							if (newTile.transform.DistanceTo (legacyTile) < 0.5f)
								nearestTile = newTile;
						}
						if (nearestTile != null) {
							decos.ForEach (deco => deco.SetParent (nearestTile.transform.GetChild (0), true));
							this.Log ("{0} decos attach to {1}", decos.Count, nearestTile.name);
						}
					}
				}
			}
			#endif
		}
	}
}