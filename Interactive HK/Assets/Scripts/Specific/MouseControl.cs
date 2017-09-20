using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        LockCursor(false);
    }
	
	// Update is called once per frame
	void Update () {
        // if ESCAPE key is pressed, then unlock the cursor
        if (Input.GetButtonDown("Cancel"))
        {
            LockCursor(false);
        }

        // if the player fires, then relock the cursor
        if (Input.GetButtonDown("Fire2"))
        {
            LockCursor(true);
        }
    }

    private void LockCursor(bool isLocked)
    {
        if (isLocked)
        {
            // make the mouse pointer invisible
            Cursor.visible = true;

            // lock the mouse pointer within the game area
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            // make the mouse pointer visible
            Cursor.visible = true;

            // unlock the mouse pointer so player can click on other windows
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
