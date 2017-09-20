using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Spotlightor.TileMap
{
	[CustomEditor (typeof(AutoTile))]
	public class AutoTileEditor : Editor
	{
		private AutoTile Tile{ get { return target as AutoTile; } }

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			List<TileSet.Element> tileChoices = Tile.tileSet.GetElementsByTileType (Tile.TileType);
			if (tileChoices.Count > 1) {
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("外观种类");
				for (int i = 0; i < tileChoices.Count; i++) {
					string buttonLabel = string.Format ("外观 {0}", i + 1);
					if (i == Tile.Choice)
						buttonLabel = string.Format (">{0}<", buttonLabel);
					if (GUILayout.Button (buttonLabel)) {
						Tile.Choice = i;
						EditorUtility.SetDirty (Tile);
						EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
					}
				}
				GUILayout.EndHorizontal ();
			}

			GUILayout.BeginHorizontal ();
			int wallCount = Tile.WallCount;
			GUILayout.Label ("Wall Count", GUILayout.Width (100));
			if (GUILayout.Button ("+", GUILayout.Width (30)))
				Tile.WallCount++;

			int newWallCount = EditorGUILayout.IntField (wallCount, GUILayout.Width (30));
			if (newWallCount != wallCount)
				Tile.WallCount = newWallCount;

			if (GUILayout.Button ("-", GUILayout.Width (30)))
				Tile.WallCount--;
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Center"))
				Tile.TileType = TileTypes.Center;
			if (GUILayout.Button ("Isolated"))
				Tile.TileType = TileTypes.Isolated;
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("EL"))
				Tile.TileType = TileTypes.EdgeLeft;
			if (GUILayout.Button ("ER"))
				Tile.TileType = TileTypes.EdgeRight;
			if (GUILayout.Button ("EF"))
				Tile.TileType = TileTypes.EdgeFront;
			if (GUILayout.Button ("EB"))
				Tile.TileType = TileTypes.EdgeBack;
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("CBL"))
				Tile.TileType = TileTypes.CornerBL;
			if (GUILayout.Button ("CBR"))
				Tile.TileType = TileTypes.CornerBR;
			if (GUILayout.Button ("CFL"))
				Tile.TileType = TileTypes.CornerFL;
			if (GUILayout.Button ("CFR"))
				Tile.TileType = TileTypes.CornerFR;
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("NCBL"))
				Tile.TileType = TileTypes.NegativeCornerBL;
			if (GUILayout.Button ("NCBR"))
				Tile.TileType = TileTypes.NegativeCornerBR;
			if (GUILayout.Button ("NCFL"))
				Tile.TileType = TileTypes.NegativeCornerFL;
			if (GUILayout.Button ("NCFR"))
				Tile.TileType = TileTypes.NegativeCornerFR;
			GUILayout.EndHorizontal ();

			serializedObject.ApplyModifiedProperties ();
		}
	}
}