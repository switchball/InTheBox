using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Spotlightor.TileMap
{
	public abstract class Tile : MonoBehaviour
	{
		public List<Vector3> bonusTileMapDatas = new List<Vector3> ();

		public List<Vector3> TileMapDatas {
			get {
				List<Vector3> tileMapDatas = new List<Vector3> (){ new Vector3 (0, 0, 0) };
				foreach (Vector3 bonusTileMapData in bonusTileMapDatas)
					tileMapDatas.Add (transform.localRotation * bonusTileMapData);
				
				return tileMapDatas;
			}
		}

		public abstract TileTypes TileType {
			get;
			set;
		}

		public abstract int WallCount {
			get;
			set;
		}
	}
}
