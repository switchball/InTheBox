using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseInteractionMessagesSource : InteractionMessagesSource
{
	protected override List<InteractionMessagesSender.InteractionPointerData> GetUiCursorDatas ()
	{
		return new List<InteractionMessagesSender.InteractionPointerData> (){new InteractionMessagesSender.InteractionPointerData(0,Input.mousePresent,Input.GetMouseButton(0), Input.mousePosition)};
	}
}
