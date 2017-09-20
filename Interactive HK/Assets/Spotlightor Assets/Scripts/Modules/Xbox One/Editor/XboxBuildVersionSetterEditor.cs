using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(XboxBuildVersionSetter))]
public class XboxBuildVersionSetterEditor :Editor
{
	private XboxBuildVersionSetter VersionSetter{ get { return target as XboxBuildVersionSetter; } }

	void OnEnable ()
	{
		SyncXboxBuildVersion ();
	}

	public override void OnInspectorGUI ()
	{
		string versionBefore = VersionSetter.version;

		DrawDefaultInspector ();

		if (versionBefore != VersionSetter.version)
			SyncXboxBuildVersion ();
	}

	public void SyncXboxBuildVersion ()
	{
		if (UnityEditor.PlayerSettings.XboxOne.Version != VersionSetter.version) {
			UnityEditor.PlayerSettings.XboxOne.Version = VersionSetter.version;
			this.Log ("XboxOne build version set to {0}", VersionSetter.version);
		}
	}
}
