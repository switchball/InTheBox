using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class PositionByLanguage : ContentByLanguage
{
	public Vector3 localPosition;
}

public class ChangePositionByLanguage : LanguageSpecifiedContent<PositionByLanguage>
{
	protected override void ActivateContent (PositionByLanguage contentByLanguage)
	{
		transform.localPosition = contentByLanguage.localPosition;
	}
}
