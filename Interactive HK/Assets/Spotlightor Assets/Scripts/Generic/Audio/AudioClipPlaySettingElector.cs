using UnityEngine;
using System.Collections;

public class AudioClipPlaySettingElector : EasyDebugGui
{
	protected override void DrawDebugGUI ()
	{
		Label (string.Format ("Audio {0}", AudioClipPlaySetting.SelectedCandidateIndex));
		if (Button ("Prev"))
			AudioClipPlaySetting.SelectedCandidateIndex--;
		if (Button ("Next"))
			AudioClipPlaySetting.SelectedCandidateIndex++;
	}
}
