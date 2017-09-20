using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Spotlightor.TileMap
{
	public class TileMap : MonoBehaviour
	{
		public class TileMapData
		{
			public int minX = 0;
			public int maxX = 0;
			public int minZ = 0;
			public int maxZ = 0;
			public int minWallCount = 0;
			public int maxWallCount = 0;
			public List<List<bool>> tileDatas = new List<List<bool>> ();

			public int xCount { get { return maxX - minX + 1; } }

			public int zCount { get { return maxZ - minZ + 1; } }
		}

		public Vector3 tileSize = new Vector3 (4, 4, 4);
		public Tile brushTile;

		public TileMapData Data {
			get {
				TileMapData data = new TileMapData ();
				Tile[] tiles = GetComponentsInChildren<Tile> ();

				foreach (Tile tile in tiles) {
					Vector3 tilePoint = LocalPositionToTilePoint (tile.transform.localPosition);
					data.minX = (int)Mathf.Min (tilePoint.x, data.minX);
					data.maxX = (int)Mathf.Max (tilePoint.x, data.maxX);
					data.minZ = (int)Mathf.Min (tilePoint.z, data.minZ);
					data.maxZ = (int)Mathf.Max (tilePoint.z, data.maxZ);
					data.minWallCount = Mathf.Min (tile.WallCount, data.minWallCount);
					data.maxWallCount = Mathf.Max (tile.WallCount, data.maxWallCount);
				}
				data.maxX++;
				data.minX--;
				data.maxZ++;
				data.minZ--;

				for (int z = 0; z < data.zCount; z++) {
					List<bool> row = new List<bool> ();

					for (int x = 0; x < data.xCount; x++)
						row.Add (false);

					data.tileDatas.Add (row);
				}

				foreach (Tile tile in tiles) {
					Vector3 tilePoint = LocalPositionToTilePoint (tile.transform.localPosition);
					int x = (int)tilePoint.x - data.minX;
					int z = (int)tilePoint.z - data.minZ;
					data.tileDatas [z] [x] = true;
				}
				return data;
			}
		}

		public void FormatTiles ()
		{
			TileMapData data = this.Data;

//			this.Log ("MinX = {0}, MaxX = {1}, MinZ = {2}, MaxZ = {3}", data.minX, data.maxX, data.minZ, data.maxZ);
			List<List<bool>> updatedTilePoints = new List<List<bool>> ();
			for (int z = 0; z < data.zCount; z++) {
				List<bool> row = new List<bool> ();
				for (int x = 0; x < data.xCount; x++)
					row.Add (false);
				updatedTilePoints.Add (row);
			}

			Tile[] tiles = GetComponentsInChildren<Tile> ();
			foreach (Tile tile in tiles) {
				Vector3 tilePoint = LocalPositionToTilePoint (tile.transform.localPosition);
				int x = (int)tilePoint.x - data.minX;
				int z = (int)tilePoint.z - data.minZ;

				bool updatedBefore = updatedTilePoints [z] [x];
				if (!updatedBefore) {
					int f = data.tileDatas [z + 1] [x] ? 1 : 0;
					int fl = data.tileDatas [z + 1] [x - 1] ? 1 : 0;
					int fr = data.tileDatas [z + 1] [x + 1] ? 1 : 0;
					int l = data.tileDatas [z] [x - 1] ? 1 : 0;
					int r = data.tileDatas [z] [x + 1] ? 1 : 0;
					int b = data.tileDatas [z - 1] [x] ? 1 : 0;
					int bl = data.tileDatas [z - 1] [x - 1] ? 1 : 0;
					int br = data.tileDatas [z - 1] [x + 1] ? 1 : 0;

					int frontSum = f + fl + fr;
					int backSum = b + bl + br;
					int leftSum = l + fl + bl;
					int rightSum = r + fr + br;

					TileTypes tileType = TileTypes.Center;

					if (frontSum <= 2 && backSum == 3 && leftSum >= 2 && rightSum >= 2)
						tileType = TileTypes.EdgeFront;
					if (frontSum == 3 && backSum <= 2 && leftSum >= 2 && rightSum >= 2)
						tileType = TileTypes.EdgeBack;
					if (frontSum >= 2 && backSum >= 2 && leftSum <= 2 && rightSum == 3)
						tileType = TileTypes.EdgeLeft;
					if (frontSum >= 2 && backSum >= 2 && leftSum == 3 && rightSum <= 2)
						tileType = TileTypes.EdgeRight;

					if (frontSum <= 1 && leftSum <= 1 && rightSum >= 2 && backSum >= 2)
						tileType = TileTypes.CornerFL;
					if (frontSum <= 1 && leftSum >= 2 && rightSum <= 1 && backSum >= 2)
						tileType = TileTypes.CornerFR;
					if (frontSum >= 2 && leftSum <= 1 && rightSum >= 2 && backSum <= 1)
						tileType = TileTypes.CornerBL;
					if (frontSum >= 2 && leftSum >= 2 && rightSum <= 1 && backSum <= 1)
						tileType = TileTypes.CornerBR;

					if (frontSum == 2 && leftSum == 2 && rightSum == 3 && backSum == 3)
						tileType = TileTypes.NegativeCornerFL;
					if (frontSum == 2 && leftSum == 3 && rightSum == 2 && backSum == 3)
						tileType = TileTypes.NegativeCornerFR;
					if (frontSum == 3 && leftSum == 2 && rightSum == 3 && backSum == 2)
						tileType = TileTypes.NegativeCornerBL;
					if (frontSum == 3 && leftSum == 3 && rightSum == 2 && backSum == 2)
						tileType = TileTypes.NegativeCornerBR;

					if (tile.TileType != tileType) {
						#if UNITY_EDITOR
						Undo.RecordObject (tile, "Change Tile Type");
						#endif
						tile.TileType = tileType;
					}

					Vector3 localPos = tilePoint;
					localPos.Scale (tileSize);
					if (tile.transform.localPosition != localPos) {
						#if UNITY_EDITOR
						Undo.RecordObject (tile, "Move tile");
						#endif
						tile.transform.localPosition = localPos;
					}

					updatedTilePoints [z] [x] = true;
				} else {
					#if UNITY_EDITOR
					this.Log ("Find overlap tile {0}, destroy it", tile.name);
					Undo.DestroyObjectImmediate (tile.gameObject);
					#endif
				}
			}
		}

		public Vector3 LocalPositionToTilePoint (Vector3 position)
		{
			return new Vector3 (
				Mathf.RoundToInt (position.x / tileSize.x),
				Mathf.RoundToInt (position.y / tileSize.y), 
				Mathf.RoundToInt (position.z / tileSize.z));
		}

		public Vector3 TilePointToLocalPosition (Vector3 localPosition)
		{
			Vector3 tilePoint = localPosition;
			tilePoint.Scale (tileSize);
			return tilePoint;
		}
	}

}