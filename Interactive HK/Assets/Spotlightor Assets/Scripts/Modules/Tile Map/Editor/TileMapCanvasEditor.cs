using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Spotlightor.TileMap
{
	[CustomEditor (typeof(TileMapCanvas), true)]
	public class TileMapCanvasEditor : Editor
	{
		private TileMapCanvas Placer{ get { return target as TileMapCanvas; } }

		void OnSceneGUI ()
		{
			bool dataChanged = false;
		
			TileMapData data = Placer.data;
			for (int z = data.RowsCount - 1; z >= 0; z--) {
				int axisZ = z + data.MinZ;
				for (int x = 0; x < data.ColumnsCount; x++) {
					int tileValue = data.TileMap [z] [x];

					int axisX = x + data.MinX;
					Vector3 localPos = Placer.tileSize;
					localPos.Scale (new Vector3 (axisX, 0, axisZ));
					localPos.y = -Placer.tileSize.x * 0.5f;
					Handles.color = new Color (1, 1, 1, 0.1f);
					if (Handles.Button (Placer.transform.TransformPoint (localPos), Quaternion.LookRotation (Placer.transform.up), Placer.tileSize.x, Placer.tileSize.x, Handles.CubeCap)) {
						tileValue++;
						if (tileValue > 1)
							tileValue = 0;
					
						data.TileMap [z] [x] = tileValue;
					
						dataChanged = true;
					}
				
				}
			}

			if (dataChanged) {
				data.SaveTileMapToTiles ();
				RebuildTileInstances ();
			}

			serializedObject.ApplyModifiedProperties ();
		}

		public override void OnInspectorGUI ()
		{
			bool needRebuild = false;

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Height", GUILayout.Width (60));
			if (GUILayout.Button ("+", GUILayout.Width (30))) {
				Placer.data.height++;
				needRebuild = true;
			}

			int newHeight = EditorGUILayout.IntField (Placer.data.height, GUILayout.Width (30));
			if (newHeight != Placer.data.height) {
				Placer.data.height = newHeight;
				needRebuild = true;
			}

			if (GUILayout.Button ("-", GUILayout.Width (30))) {
				Placer.data.height--;
				Placer.data.height = Mathf.Max (0, Placer.data.height);
				needRebuild = true;
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Rebuild Mesh"))
				needRebuild = true;
			if (GUILayout.Button ("Clear Mesh"))
				ClearTileInstances ();

			if (GUILayout.Button ("Reset", GUILayout.Width (60))) {
				Placer.data.ResetTiles ();
				needRebuild = true;
			}
			GUILayout.EndHorizontal ();

			if (needRebuild)
				RebuildTileInstances ();

			DrawDefaultInspector ();
		
			serializedObject.ApplyModifiedProperties ();
		}

		private void RebuildTileInstances ()
		{
			ClearTileInstances ();
		
			Dictionary<TileTypes, List<TileMapCanvas.TilePrefabSetting>> tileByTypes = new Dictionary<TileTypes, List<TileMapCanvas.TilePrefabSetting>> ();
			tileByTypes [TileTypes.Center] = Placer.centers;
			tileByTypes [TileTypes.EdgeFront] = Placer.edgesFront;
			tileByTypes [TileTypes.EdgeBack] = Placer.edgesBack;
			tileByTypes [TileTypes.EdgeLeft] = Placer.edgesLeft;
			tileByTypes [TileTypes.EdgeRight] = Placer.edgesRight;
			tileByTypes [TileTypes.CornerFL] = Placer.cornersFL;
			tileByTypes [TileTypes.CornerFR] = Placer.cornersFR;
			tileByTypes [TileTypes.CornerBL] = Placer.cornersBL;
			tileByTypes [TileTypes.CornerBR] = Placer.cornersBR;
			tileByTypes [TileTypes.NegativeCornerFL] = Placer.negativeCornersFL;
			tileByTypes [TileTypes.NegativeCornerFR] = Placer.negativeCornersFR;
			tileByTypes [TileTypes.NegativeCornerBL] = Placer.negativeCornersBL;
			tileByTypes [TileTypes.NegativeCornerBR] = Placer.negativeCornersBR;

			TileMapData data = Placer.data;

			for (int z = 1; z < data.RowsCount - 1; z++) {
				for (int x = 1; x < data.ColumnsCount - 1; x++) {
					int tileValue = data.TileMap [z] [x];
					if (tileValue > 0) {
						int f = data.TileMap [z + 1] [x];
						int fl = data.TileMap [z + 1] [x - 1];
						int fr = data.TileMap [z + 1] [x + 1];
						int l = data.TileMap [z] [x - 1];
						int r = data.TileMap [z] [x + 1];
						int b = data.TileMap [z - 1] [x];
						int bl = data.TileMap [z - 1] [x - 1];
						int br = data.TileMap [z - 1] [x + 1];
					
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

						int tilePrefabSettingsIndex = 0;
						TileMapCanvas.TilePrefabSetting tilePrefabSetting = tileByTypes [tileType] [tilePrefabSettingsIndex];
						Transform instance = PrefabUtility.InstantiatePrefab (tilePrefabSetting.top) as Transform;
					
						instance.SetParent (Placer.transform, false);
						Vector3 localPos = new Vector3 (Placer.tileSize.x, 0, Placer.tileSize.z);
						localPos.Scale (new Vector3 (x + data.MinX, 0, z + data.MinZ));
						instance.localPosition = localPos;
						instance.localEulerAngles = tilePrefabSetting.topRotation;
					
						if (Placer.data.height > 0 && tileType != TileTypes.Center) {
							for (int h = 1; h <= Placer.data.height; h++) {							
								Transform sideInstance = PrefabUtility.InstantiatePrefab (tilePrefabSetting.wall) as Transform;
							
								sideInstance.SetParent (Placer.transform, false);
								localPos.y += Placer.tileSize.y;
								sideInstance.localPosition = localPos;
								sideInstance.localEulerAngles = tilePrefabSetting.wallRotation;
							}
						}
					}
				}
			}

			EditorUtility.SetDirty (target);
			#if UNITY_5
			UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty (UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene ());
			#else
			EditorApplication.MarkSceneDirty ();
			#endif
		}

		private void ClearTileInstances ()
		{
			while (Placer.transform.childCount > 0)
				DestroyImmediate (Placer.transform.GetChild (0).gameObject);
		}

	}
}