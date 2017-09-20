using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject player;
    public Canvas mainCanvas;

    public RawImage title;
    public RawImage instructions;

    public Button startButton;
    public Button exitButton;
    public Button nextButton;

    private void Start()
    {
        player.GetComponent<Controller>().enabled = false;
        player.GetComponent<MouseLooker>().enabled = false;
    }

    public void OnPressStart () {
        if (title.enabled)
        {
            title.gameObject.SetActive(false);
            instructions.gameObject.SetActive(true);
            startButton.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);
        }
	}

    public void OnPressNext()
    {

            mainCanvas.enabled = false;
            player.GetComponent<Controller>().enabled = true;
            player.GetComponent<MouseLooker>().enabled = true;

    }

    // Update is called once per frame
    public void OnPressExit () {
        Application.Quit(); 
	}
}
