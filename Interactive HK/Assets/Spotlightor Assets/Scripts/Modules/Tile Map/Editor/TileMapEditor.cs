using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Spotlightor.TileMap
{
	[CustomEditor (typeof(TileMap), true)]
	public class TileMapEditor : Editor
	{
		public TileMap Map{ get { return target as TileMap; } }

		private static Color BrushButtonColorEmpty = new Color (0, 1, 0, 0.4f);

		private static Color BrushButtonColorDrawn = new Color (1, 0, 0, 0.4f);

		void OnSceneGUI ()
		{
			TileMap.TileMapData data = Map.Data;
			List<Tile> tiles = new List<Tile> (Map.GetComponentsInChildren<Tile> ());

			for (int z = 0; z < data.zCount; z++) {
				for (int x = 0; x < data.xCount; x++) {
					Vector3 localPos = new Vector3 (x + data.minX, 0, z + data.minZ);
					localPos.Scale (Map.tileSize);
					localPos.y = -Map.tileSize.x * 0.5f;
					bool isEmpty = data.tileDatas [z] [x] == false;
					Handles.color = isEmpty ? BrushButtonColorEmpty : BrushButtonColorDrawn;
					float buttonScale = isEmpty ? 1 : 0.9f;
					if (Handles.Button (
						    Map.transform.TransformPoint (localPos), 
						    Quaternion.LookRotation (Map.transform.up), 
						    Map.tileSize.x * 0.5f * buttonScale, 
						    Map.tileSize.x * 0.5f * buttonScale, 
						    Handles.RectangleCap)) {
						int tileDestroyed = 0;
						foreach (Tile tile in tiles) {
							Vector3 tilePoint = Map.LocalPositionToTilePoint (tile.transform.localPosition);
							int tileX = (int)tilePoint.x - data.minX;
							int tileZ = (int)tilePoint.z - data.minZ;
							if (tileX == x && tileZ == z) {
								DestroyImmediate (tile.gameObject);
								tileDestroyed++;
							}
						}

						if (tileDestroyed == 0) {
							Tile tileInstance = PrefabUtility.InstantiatePrefab (Map.brushTile) as Tile;
							tileInstance.transform.SetParent (Map.transform);
							tileInstance.transform.localEulerAngles = Vector3.zero;
							tileInstance.transform.localPosition = new Vector3 (localPos.x, 0, localPos.z);
							tileInstance.WallCount = data.maxWallCount;
						}
					}
				}
			}

			Map.FormatTiles ();
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			List<Tile> tiles = new List<Tile> (Map.GetComponentsInChildren<Tile> ());

			DrawWallCountGui (tiles);

			DrawPivotMovementGui (tiles);

			DrawLineCreateGui (tiles);

			DrawLineDeleteGui (tiles);

			if (GUILayout.Button ("重排所有元素位置")) {
				foreach (Tile tile in tiles) {
					if (tile is AutoTile)
						(tile as AutoTile).UpdateTileMeshesPositions ();
				}
				EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
			}
		}


		private void DrawWallCountGui (List<Tile> tiles)
		{
			int wallCount = 0;
			foreach (Tile tile in tiles)
				wallCount = Mathf.Max (wallCount, tile.WallCount);

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("高度", GUILayout.Width (60));
			if (GUILayout.Button ("+", GUILayout.Width (30))) {
				tiles.ForEach (tile => tile.WallCount++);
				tiles.ForEach (tile => EditorUtility.SetDirty (tile));
				EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
			}

			int newWallCount = EditorGUILayout.IntField (wallCount, GUILayout.Width (30));
			if (newWallCount != wallCount) {
				tiles.ForEach (tile => tile.WallCount = newWallCount);
				tiles.ForEach (tile => EditorUtility.SetDirty (tile));
				EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
			}

			if (GUILayout.Button ("-", GUILayout.Width (30))) {
				tiles.ForEach (tile => tile.WallCount--);
				tiles.ForEach (tile => EditorUtility.SetDirty (tile));
				EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
			}
			GUILayout.EndHorizontal ();
		}

		private void DrawPivotMovementGui (List<Tile> tiles)
		{
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("整体移动");
			DrawCenterButton (tiles, ">居中<");
			GUILayout.Space (10);
			DrawOffsetButton (tiles, Vector3.left, "左移");
			DrawOffsetButton (tiles, Vector3.right, "右移");
			DrawOffsetButton (tiles, Vector3.forward, "前移");
			DrawOffsetButton (tiles, Vector3.back, "后移");
			GUILayout.EndHorizontal ();
		}

		private void DrawOffsetButton (List<Tile> tiles, Vector3 offsetPoint, string label)
		{
			if (GUILayout.Button (label)) {
				Vector3 offset = offsetPoint;
				offset.Scale (Map.tileSize);
				tiles.ForEach (tile => tile.transform.localPosition += offset);

				EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
			}
		}

		private void DrawCenterButton (List<Tile> tiles, string label)
		{
			if (GUILayout.Button (label)) {
				Vector3 minPos = Vector3.zero;
				Vector3 maxPos = Vector3.zero;
				foreach (Tile tile in tiles) {
					minPos = Vector3.Min (tile.transform.localPosition, minPos);
					maxPos = Vector3.Max (tile.transform.localPosition, maxPos);
				}
				Vector3 centerPos = (maxPos + minPos) * 0.5f;
				centerPos = Map.LocalPositionToTilePoint (centerPos);
				centerPos.Scale (Map.tileSize);
				tiles.ForEach (tile => tile.transform.localPosition -= centerPos);

				Map.transform.position += Map.transform.InverseTransformVector (centerPos);

				EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
			}
		}

		private void DrawLineDeleteGui (List<Tile> tiles)
		{
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("删除整行");
			DrawLineDeleteButton (tiles, Vector3.left, "左-");
			DrawLineDeleteButton (tiles, Vector3.right, "右-");
			DrawLineDeleteButton (tiles, Vector3.forward, "前-");
			DrawLineDeleteButton (tiles, Vector3.back, "后-");
			GUILayout.EndHorizontal ();
		}

		private void DrawLineDeleteButton (List<Tile> tiles, Vector3 deleteAxis, string label)
		{
			if (GUILayout.Button (label)) {
				TileMap.TileMapData data = Map.Data;
				float targetValue = 0;
				Vector3 mask = Vector3.zero;
				if (deleteAxis == Vector3.left)
					targetValue = data.minX + 1;
				else if (deleteAxis == Vector3.right)
					targetValue = data.maxX - 1;
				else if (deleteAxis == Vector3.forward)
					targetValue = data.maxZ - 1;
				else if (deleteAxis == Vector3.back)
					targetValue = data.minZ + 1;
				else
					this.LogWarning ("Line Delete does not support deleteAxis: {0}", deleteAxis);

				if (deleteAxis.x != 0)
					mask = Vector3.right;
				else if (deleteAxis.z != 0)
					mask = Vector3.forward;

				List<Tile> tilesToDelete = new List<Tile> ();

				foreach (Tile tile in tiles) {
					Vector3 mapPoint = Map.LocalPositionToTilePoint (tile.transform.localPosition);
					mapPoint.Scale (mask);
					float value = mapPoint.x + mapPoint.y + mapPoint.z;
					if (value == targetValue) {
						tilesToDelete.Add (tile);
					}
				}

				foreach (Tile tile in tilesToDelete) {
					DestroyImmediate (tile.gameObject);
					tiles.Remove (tile);
				}

				EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
			}
		}

		private void DrawLineCreateGui (List<Tile> tiles)
		{
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("增加整行");
			DrawLineCreateButton (tiles, Vector3.left, "左+");
			DrawLineCreateButton (tiles, Vector3.right, "右+");
			DrawLineCreateButton (tiles, Vector3.forward, "前+");
			DrawLineCreateButton (tiles, Vector3.back, "后+");
			GUILayout.EndHorizontal ();
		}

		private void DrawLineCreateButton (List<Tile> tiles, Vector3 lineAxis, string label)
		{
			if (GUILayout.Button (label)) {
				TileMap.TileMapData data = Map.Data;
				Vector3 staticPart = Vector3.zero;
				Vector3 increasePart = Vector3.forward;
				if (lineAxis == Vector3.left)
					staticPart = Vector3.right * data.minX;
				else if (lineAxis == Vector3.right)
					staticPart = Vector3.right * data.maxX;
				else if (lineAxis == Vector3.forward)
					staticPart = Vector3.forward * data.maxZ;
				else if (lineAxis == Vector3.back)
					staticPart = Vector3.forward * data.minZ;
				else
					this.LogWarning ("Line Create does not support axis: {0}", lineAxis);

				int increaseStart = 0;
				int increaseEnd = 0;
				if (lineAxis.x != 0) {
					increasePart = Vector3.forward;
					increaseStart = data.minZ + 1;
					increaseEnd = data.maxZ - 1;
				} else if (lineAxis.z != 0) {
					increasePart = Vector3.right;
					increaseStart = data.minX + 1;
					increaseEnd = data.maxX - 1;
				}

				for (int i = increaseStart; i <= increaseEnd; i++) {
					Vector3 tilePoint = i * increasePart + staticPart;
					InstantiateTileAtTilePoint (tilePoint);
				}

				EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
			}
		}

		private void InstantiateTileAtTilePoint (Vector3 mapPoint)
		{
			Tile tileInstance = PrefabUtility.InstantiatePrefab (Map.brushTile) as Tile;
			tileInstance.transform.SetParent (Map.transform);
			tileInstance.transform.localEulerAngles = Vector3.zero;
			tileInstance.transform.localPosition = Map.TilePointToLocalPosition (mapPoint);
			tileInstance.WallCount = Map.Data.maxWallCount;
		}

	}

}