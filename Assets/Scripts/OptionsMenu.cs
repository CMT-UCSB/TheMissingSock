using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public GameManager gm;

    public void OnResume()
    {
        gm.battling = false;
    }

    public void OnRestart()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OnQuit()
    {
        Application.Quit(); 
    }
}
