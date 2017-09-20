using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GosActivatorOnLocalizationLanguage : GosActivatorOnCondition
{
	public List<LocalizationLanguageTypes> activeLanguages;

	protected override bool HasMetCondition { get { return activeLanguages.Contains (Localization.CurrentLanguage); } }

	protected override void OnEnable ()
	{
		base.OnEnable ();
		Messenger.AddListener (Localization.MessageLanguageChanged, OnLanguageChanged);
	}

	void OnDisable ()
	{
		Messenger.RemoveListener (Localization.MessageLanguageChanged, OnLanguageChanged);
	}

	protected void OnLanguageChanged (object data)
	{
		this.ActivateObjectsOnCondition ();
	}
}
