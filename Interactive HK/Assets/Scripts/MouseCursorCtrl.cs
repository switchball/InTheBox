using UnityEngine;
using System.Collections;

public class MouseCursorCtrl : MonoBehaviour {

	
	// internal private variables
	private Quaternion m_CharacterTargetRot;
	private Quaternion m_CameraTargetRot;
	private Transform character;
	private Transform cameraTransform;

    private float m_SmoothTime;
    private float m_DisableInputTimeLeft;

	void Start() {
		// start the game with the cursor locked
		LockCursor (true);
        StartCoroutine(L());

		// get a reference to the character's transform (which this script should be attached to)
		character = gameObject.transform;

		// get a reference to the main camera's transform
		cameraTransform = Camera.main.transform;

		// get the location rotation of the character and the camera
		m_CharacterTargetRot = character.localRotation;
		m_CameraTargetRot = cameraTransform.localRotation;
        
	}

    IEnumerator L()
    {
        yield return new WaitForSeconds(0.5f);
        LockCursor(true);
    }

    void Update() {
        // whether disable input
        if (m_DisableInputTimeLeft > 0)
        {
            m_DisableInputTimeLeft -= Time.deltaTime;
        }

		// if ESCAPE key is pressed, then unlock the cursor
		if(Input.GetButtonDown("Cancel")){
			LockCursor (false);
		}
    }
	
	public void LockCursor(bool isLocked)
	{
		if (isLocked) 
		{
			// make the mouse pointer invisible
			Cursor.visible = false;

			// lock the mouse pointer within the game area
			Cursor.lockState = CursorLockMode.Locked;
		} else {
			// make the mouse pointer visible
			Cursor.visible = true;

			// unlock the mouse pointer so player can click on other windows
			Cursor.lockState = CursorLockMode.None;
		}
	}

    public void DisableMove()
    {
        m_DisableInputTimeLeft = 1000000;
        //LockCursor(false);
    }

    public void EnableMove()
    {
        m_DisableInputTimeLeft = 0;
        LockCursor(true);
    }
    
}
