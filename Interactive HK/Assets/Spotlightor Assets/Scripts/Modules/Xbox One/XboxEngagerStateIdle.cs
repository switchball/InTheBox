using UnityEngine;
using System.Collections;

public class XboxEngagerStateIdle : XboxEngager.EngageState
{
	public override XboxEngager.StateTypes StateId { get { return XboxEngager.StateTypes.Idle; } }
}
