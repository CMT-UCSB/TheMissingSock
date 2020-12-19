using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Canvas pause;

    private bool paused = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!paused)
            {
                pause.gameObject.SetActive(true);
            }

            else
            {
                pause.gameObject.SetActive(false);
            }

            paused = !paused;
        }
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
