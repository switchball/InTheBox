using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Spotlightor.TileMap
{
	public class AutoTile : Tile
	{
		[SerializeField]
		private TileTypes tileType = TileTypes.Isolated;
		[SerializeField]
		private int choice = 0;
		[SerializeField]
		private int wallCount = 0;
		public TileSet tileSet;

		public override TileTypes TileType {
			get { return tileType; }
			set {
				if (tileType != value) {
					tileType = value;
					RebuildAll ();
				}
			}
		}

		public override int WallCount {
			get { return wallCount; }
			set {
				value = Mathf.Max (value, 0);
				if (wallCount != value) {
					wallCount = value;
					BuildWallInstances (wallCount);
				}
			}
		}

		public int Choice {
			get { return choice; }
			set {
				choice = value;
				RebuildAll ();
			}
		}

		public bool HasBottom {
			get { return tileSet.GetElementsByTileType (this.tileType) [choice].bottom != null; }
		}

		[ContextMenu ("Rebuild")]
		public void RebuildAll ()
		{
			while (transform.childCount > 0)
				DestroyInstance (transform.GetChild (0).gameObject);

			List<TileSet.Element> elements = tileSet.GetElementsByTileType (this.tileType);
			choice = Mathf.Clamp (choice, 0, elements.Count - 1);
			TileSet.Element choiceElement = elements [choice];

			TileMesh top = InstantiatePrefab (choiceElement.top) as TileMesh;

			top.transform.SetParent (this.transform, false);
			top.transform.localPosition = Vector3.zero;
			top.transform.localEulerAngles = choiceElement.topRotation;

			if (HasBottom) {
				TileMesh bottom = InstantiatePrefab (choiceElement.bottom) as TileMesh;

				bottom.transform.SetParent (this.transform, false);
				bottom.transform.localPosition = Vector3.zero;
				bottom.transform.localEulerAngles = choiceElement.bottomRotation;
			}

			RebuildWalls (wallCount);
		}

		protected void RebuildWalls (int targetWallCount)
		{
			int notWallChildrenCount = HasBottom ? 2 : 1;
			while (transform.childCount > notWallChildrenCount)
				DestroyInstance (transform.GetChild (1).gameObject);

			BuildWallInstances (targetWallCount);
		}

		protected void BuildWallInstances (int targetWallCount)
		{
			int notWallChildrenCount = HasBottom ? 2 : 1;
			int childWallCount = Mathf.Max (transform.childCount - notWallChildrenCount, 0);
			if (childWallCount > targetWallCount) {
				int deleteCount = childWallCount - targetWallCount;
				for (int i = 0; i < deleteCount; i++)
					DestroyInstance (transform.GetChild (1).gameObject);
			} else if (childWallCount < targetWallCount) {
				List<TileSet.Element> elements = tileSet.GetElementsByTileType (this.tileType);
				TileSet.Element choiceElement = elements [choice];

				int newWallsCount = targetWallCount - childWallCount;
				for (int i = 0; i < newWallsCount; i++) {
					if (choiceElement.wall != null) {
						TileMesh instance = InstantiatePrefab (choiceElement.wall) as TileMesh;

						instance.transform.SetParent (this.transform, false);
						instance.transform.SetSiblingIndex (transform.childCount - notWallChildrenCount);
						instance.transform.localEulerAngles = choiceElement.wallRotation;
					}
				}
			}

			UpdateTileMeshesPositions ();
		}

		public void UpdateTileMeshesPositions ()
		{
			TileMesh[] tileMeshes = GetComponentsInChildren<TileMesh> ();
			Vector3 localPos = Vector3.zero;
			foreach (TileMesh tileMesh in tileMeshes) {
				tileMesh.transform.localPosition = localPos;
				localPos.y += tileMesh.height;
			}
		}

		private Object InstantiatePrefab (Object target)
		{
			#if UNITY_EDITOR
			Object instance = PrefabUtility.InstantiatePrefab (target);
//			Undo.RegisterCreatedObjectUndo (instance, "Instantiate " + instance.name);
			return instance;
			#else
			return Instantiate(target);
			#endif
		}

		private void DestroyInstance (Object target)
		{
			#if UNITY_EDITOR
			DestroyImmediate (target);
//			Undo.DestroyObjectImmediate (target);
			#else
			Destroy(target);
			#endif
		}
	}
}
