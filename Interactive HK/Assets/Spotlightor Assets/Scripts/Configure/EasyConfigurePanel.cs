using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
public class EasyConfigurePanel : MonoBehaviour
{
	[System.Serializable]
	public class EasyConfigureStringGui : EasyConfigureGui
	{
		protected override void DrawCustomGui (Vector2 topLeftPosition)
		{
			string value = EasyConfigure.GetString (key);
			value = GUI.TextField (new Rect (topLeftPosition.x, topLeftPosition.y, 300, 30), value);
			EasyConfigure.SetString (key, value);
		}
		
		public override void Save ()
		{
			EasyConfigure.SaveString (key);
		}
	}

	[System.Serializable]
	public class EasyConfigureIntGui : EasyConfigureGui
	{
		public int[] modifySteps = new int[]{1};
		public int buttonWidth = 30;

		protected override void DrawCustomGui (Vector2 topLeftPosition)
		{
			int value = EasyConfigure.GetInt (key);
			string displayText = string.Format ("{0}", value);
			GUI.Box (new Rect (topLeftPosition.x, topLeftPosition.y, 100, 30), displayText);
			topLeftPosition.x += 110;

			foreach (int modifyStep in modifySteps) {
				string stepText = modifySteps.Length == 1 ? "" : modifyStep.ToString ();

				if (GUI.Button (new Rect (topLeftPosition.x, topLeftPosition.y, buttonWidth, 30), "+" + stepText))
					value += modifyStep;
				topLeftPosition.x += buttonWidth + 5;
			
				if (GUI.Button (new Rect (topLeftPosition.x, topLeftPosition.y, buttonWidth, 30), "-" + stepText))
					value -= modifyStep;
				topLeftPosition.x += buttonWidth + 5;
			}
			
			EasyConfigure.SetInt (key, value);
		}
		
		public override void Save ()
		{
			EasyConfigure.SaveInt (key);
		}
	}

	[System.Serializable]
	public class EasyConfigureFloatGui : EasyConfigureGui
	{
		public float modifyStep = 0.1f;

		protected override void DrawCustomGui (Vector2 topLeftPosition)
		{
			float value = EasyConfigure.GetFloat (key);
			string displayText = string.Format ("{0:0.00}", value);
			GUI.Box (new Rect (topLeftPosition.x, topLeftPosition.y, 100, 30), displayText);
			topLeftPosition.x += 110;

			if (GUI.Button (new Rect (topLeftPosition.x, topLeftPosition.y, 30, 30), "+"))
				value += modifyStep;
			topLeftPosition.x += 35;

			if (GUI.Button (new Rect (topLeftPosition.x, topLeftPosition.y, 30, 30), "-"))
				value -= modifyStep;

			EasyConfigure.SetFloat (key, value);
		}

		public override void Save ()
		{
			EasyConfigure.SaveFloat (key);
		}
	}

	[System.Serializable]
	public abstract class EasyConfigureGui
	{
		public string label;
		public string key;
		
		public void DrawGui (Vector2 topLeftPosition)
		{
			GUI.Box (new Rect (topLeftPosition.x, topLeftPosition.y, 120, 30), string.IsNullOrEmpty (label) ? key : label);
			DrawCustomGui (topLeftPosition + new Vector2 (125, 0));
		}

		protected abstract void DrawCustomGui (Vector2 topLeftPosition);

		public abstract void Save ();
	}
	public GUISkin guiSkin;
	public KeyCode showHideKey = KeyCode.C;
	public bool visible = false;
	public List<EasyConfigureIntGui> configureIntGuis;
	public List<EasyConfigureFloatGui> configureFloatGuis;
	public List<EasyConfigureStringGui> configureStringGuis;

	void Update ()
	{
		if (Input.GetKeyDown (showHideKey)) {
			visible = !visible;
			if (visible == false)
				SaveConfigure ();
		}
	}

	void OnGUI ()
	{
		if (visible) {
			if (guiSkin != null)
				GUI.skin = guiSkin;

			Vector2 topLeft = Vector2.zero;

			GUI.Box (new Rect (0, 0, 300, 30), string.Format ("Press '{0}' to save configuration.", showHideKey.ToString ()));
			topLeft.y += 35;
			foreach (EasyConfigureIntGui gui in configureIntGuis) {
				gui.DrawGui (topLeft);
				topLeft.y += 35;
			}
			foreach (EasyConfigureFloatGui gui in configureFloatGuis) {
				gui.DrawGui (topLeft);
				topLeft.y += 35;
			}
			foreach (EasyConfigureStringGui gui in configureStringGuis) {
				gui.DrawGui (topLeft);
				topLeft.y += 35;
			}
		}
	}

	private void SaveConfigure ()
	{
		configureIntGuis.ForEach (gui => gui.Save ());
		configureFloatGuis.ForEach (gui => gui.Save ());
		configureStringGuis.ForEach (gui => gui.Save ());
	}
}
