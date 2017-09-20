using UnityEngine;
using System.Collections;

// A SaveEngine that do nothing. Can be used as fallback engine.
public class DummySaveEngine : SaveEngine
{
	protected override void DoInstall ()
	{
		OnInstallationCompleted (true);
	}

	protected override void DoUninstall ()
	{
		OnUninstallationCompleted (true);
	}

	public override void Save (string slotName, string saveDataString)
	{
	}

	public override void Load (string slotName)
	{
	}

	public override void Delete (string slotName)
	{
	}
}
