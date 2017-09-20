﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

#if UNITY_XBOXONE
using UnityEtx;
#endif

namespace UnityEditor
{
	public static class DataPlatformEditorExtensions
	{
		#if UNITY_XBOXONE
		private const string XboxEventManifestFileNameKeyword = "xbox_events";

		[MenuItem ("XboxOne/Generate Events (filename: xbox_events)")]
		public static void GenerateEventBindings ()
		{
			// We generate the templates.xml directly next to our editor extension.
			//string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
			XCETemplates.InitializeTemplates ();

			var potentialManifests = AssetDatabase.GetAllAssetPaths ().Where (p => Path.GetFileName (p).Contains (XboxEventManifestFileNameKeyword));

			bool didParse = false;
			XCEFile xceManifest = new XCEFile (true);

			foreach (String file in potentialManifests) {
				string filename = Path.GetFullPath (file);
				if (xceManifest.ParseFile (filename)) {
					didParse = true;
					Debug.Log ("Parsing XCE Manifest: " + file);
					break;
				} else {
					Debug.Log ("Failed parsing: " + file);
					// Just ensure our state is clean if this guy didn't parse.
					xceManifest = new XCEFile ();
				}
			}
			
			if (didParse) {
				string path = "XboxOne_Generated_DataPlatform";
				string apath = (Application.dataPath + "/" + path).Replace ("/", "\\");

				if (!Directory.Exists (apath)) {
					AssetDatabase.CreateFolder ("Assets", path);
				}

				UnityCSLiveServicesDynamicEventWrapper cs = new UnityCSLiveServicesDynamicEventWrapper (xceManifest);
				cs.Generate (Path.Combine (apath, "EventWrappers.cs"));

				// Now refresh so the new assets appear in the DB.
				AssetDatabase.Refresh ();
			}
		}
		#endif
	}

}
