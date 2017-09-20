using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FeedbackPromptResetByVersionBase : MonoBehaviour
{
	private const string LastVersionSaveKey = "fdbk_last_version";
	public List<FeedbackPrompt> prompts;

	protected abstract string CurrentVersion{ get; }

	void Start ()
	{
		string lastVersion = BasicDataTypeStorage.GetString (LastVersionSaveKey);
		if (lastVersion != CurrentVersion) {
			prompts.ForEach (p => p.Reset ());
			this.Log ("FeedbackPrompt reset because current version [{0}] != last version [{1}]", CurrentVersion, lastVersion);
			BasicDataTypeStorage.SetString (LastVersionSaveKey, CurrentVersion);
		}
	}
}
