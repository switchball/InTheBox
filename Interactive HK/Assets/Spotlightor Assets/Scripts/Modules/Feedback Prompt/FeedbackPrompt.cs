using UnityEngine;
using System.Collections;
using System;

public class FeedbackPrompt : ScriptableObject
{
	public int daysUntilPrompt = 1;
	public int usesUntilPrompt = 5;
	[SingleLineLabel()]
	public int
		significantEventsUntilPrompt = 0;
	private SavableInt usesSaver;
	private SavableInt significantEventsSaver;
	private SavableBool hasAcceptedSaver;

	public int Uses {
		get { return UsesSaver.Value;}
		set { UsesSaver.Value = Mathf.Max (value, 0);}
	}

	private SavableInt UsesSaver {
		get {
			if (usesSaver == null)
				usesSaver = new SavableInt (name + "_us", 0);
			return usesSaver;
		}
	}

	public int SignificantEvents {
		get { return SignificantEventsSaver.Value;}
		set { SignificantEventsSaver.Value = Mathf.Max (value, 0);}
	}

	private SavableInt SignificantEventsSaver {
		get {
			if (significantEventsSaver == null)
				significantEventsSaver = new SavableInt (name + "_se", 0);
			return significantEventsSaver;
		}
	}

	public bool HasAccepted {
		get { return HasAcceptedSaver.Value;}
		set {
			HasAcceptedSaver.Value = value;
			if (value)
				this.Log ("Feedback prompt {0} accepted, won't promote in this version anymore.", name);
		}
	}

	private SavableBool HasAcceptedSaver {
		get {
			if (hasAcceptedSaver == null)
				hasAcceptedSaver = new SavableBool (name + "_acpt", false);
			return hasAcceptedSaver;
		}
	}

	public bool IsReadyToPrompt {
		get {
			return HasAcceptedSaver == false
				&& Days >= daysUntilPrompt
				&& UsesSaver >= usesUntilPrompt
				&& SignificantEventsSaver >= significantEventsUntilPrompt;
		}
	}

	public int Days {
		get { return Mathf.FloorToInt ((float)DateTime.Now.Subtract (LastPromptDateTime).TotalDays);}
	}
	
	public DateTime LastPromptDateTime {
		set { BasicDataTypeStorage.SetDateTimeDay (LastPromptDateTimeSaveKey, value);}
		get { return BasicDataTypeStorage.GetDateTimeDay (LastPromptDateTimeSaveKey);}
	}

	private string LastPromptDateTimeSaveKey{ get { return name + "_dt"; } }

	public void Reset ()
	{
		Uses = 0;
		SignificantEvents = 0;
		LastPromptDateTime = DateTime.Now;
		HasAccepted = false;

		this.Log ("Feedback prompt {0} reset", name);
	}
}
