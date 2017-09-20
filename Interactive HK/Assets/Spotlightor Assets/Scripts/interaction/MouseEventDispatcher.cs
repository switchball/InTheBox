using UnityEngine;
using System.Collections;

public class MouseEventDispatcher : RealClickListener
{
	public delegate void GenericMouseEventHandler (MouseEventDispatcher source);

	public event GenericMouseEventHandler HoverIn;
	public event GenericMouseEventHandler HoverOut;
	public event GenericMouseEventHandler PressDown;
	public event GenericMouseEventHandler PressUp;
	public event GenericMouseEventHandler Hover;
	public event GenericMouseEventHandler Drag;

	public bool IsPressedDown{ get; private set; }

	public bool IsHovering{ get; private set; }

	public int InteractingPointerId{ get; private set; }
	
	protected override void OnPressDown (InteractionMessagesSender.PointerMessageData pointerMessageData)
	{
		base.OnPressDown (pointerMessageData);
		
		IsPressedDown = true;
		
		if (PressDown != null)
			PressDown (this);
	}
	
	protected virtual void OnPressUp (InteractionMessagesSender.PointerMessageData pointerMessageData)
	{
		IsPressedDown = false;

		if (PressUp != null)
			PressUp (this);
	}

	protected virtual void OnHoverIn (InteractionMessagesSender.PointerMessageData pointerMessageData)
	{
		InteractingPointerId = pointerMessageData.pointerId;
		IsHovering = true;
		
		if (HoverIn != null)
			HoverIn (this);
	}

	protected virtual void OnHoverOut (InteractionMessagesSender.PointerMessageData pointerMessageData)
	{
		InteractingPointerId = -1;
		IsHovering = false;
		
		if (HoverOut != null)
			HoverOut (this);
	}

	protected virtual void OnHover (InteractionMessagesSender.PointerMessageData pointerMessageData)
	{
		if (Hover != null)
			Hover (this);
	}

	protected virtual void OnDrag (InteractionMessagesSender.PointerMessageData pointerMessageData)
	{
		if (Drag != null)
			Drag (this);
	}

	void OnDisable ()
	{
		IsPressedDown = false;
		IsHovering = false;
		InteractingPointerId = -1;
	}
}
